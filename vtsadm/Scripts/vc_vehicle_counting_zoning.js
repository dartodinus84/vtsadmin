/* Vehicle Counting Zoning - loaded by mst_vehicle_counting.aspx */
(function (window, $) {
    'use strict';

    var vcZoningState = window.vcZoningState = {
        id: 0,
        companyName: '',
        vehicleId: '',
        nopol: '',
        gpsSn: '',
        channelRaw: '',
        channelText: '',
        activeChannels: [],
        zoningData: {},
        previewDataUrl: {},
        selectedChannel: '',
        imageLoadedMap: {}
    };

    function vcLog() {
        if (window.vcZoningDebug && window.console) {
            console.log.apply(console, arguments);
        }
    }

    function vcShowAlert(icon, title, text) {
        if (typeof Swal !== 'undefined') {
            Swal.fire({ icon: icon, title: title, text: text, confirmButtonColor: '#003481' });
        } else {
            alert(text || title);
        }
    }

    function parseChannelList(channelText) {
        if (!channelText) return [];
        return channelText.split(',').map(function (c) { return c.trim(); }).filter(function (c) { return c !== ''; });
    }

    function formatChannelLabel(channelKey) {
        return 'CH ' + channelKey;
    }

    function defaultImagePath(gpsSn, channelKey) {
        return 'Picture/zoning/' + gpsSn + '/channel_' + channelKey + '.jpg';
    }

    function resolveZoningImageUrl(relativePath) {
        if (!relativePath) return '';
        var path = relativePath.trim().replace(/\\/g, '/');
        if (/^https?:\/\//i.test(path)) return path;
        if (path.charAt(0) === '/') return path;
        var root = window.vcAppRoot || '/';
        if (root.charAt(root.length - 1) !== '/') root += '/';
        return root + path.replace(/^\/+/, '');
    }

    function buildZoningImagePreviewUrl(imagePath) {
        if (!imagePath) return '';
        var path = imagePath.trim().replace(/\\/g, '/');
        if (/^data:/i.test(path)) return path;
        return resolveZoningImageUrl(path) + '?v=' + new Date().getTime();
    }

    function normalizeImagePath(path) {
        if (!path) return '';
        var p = path.trim().replace(/\\/g, '/');
        if (/^[a-zA-Z]:\//.test(p) || p.indexOf(':/') > 0 && p.indexOf('Picture/') < 0) return '';
        p = p.replace(/^\/+/, '');
        if (p.toLowerCase().indexOf('picture/zoning/') === 0) return p;
        if (p.toLowerCase().indexOf('damri_zoning/') === 0) {
            return 'Picture/zoning/' + p.substring('damri_zoning/'.length);
        }
        return p;
    }

    function normalizeZoneArray(zone) {
        if (!Array.isArray(zone)) return [];
        var result = [];
        for (var i = 0; i < zone.length; i++) {
            var pt = zone[i];
            if (!Array.isArray(pt) || pt.length < 2) continue;
            var x = parseFloat(pt[0]);
            var y = parseFloat(pt[1]);
            if (isNaN(x) || isNaN(y)) continue;
            result.push([parseFloat(x.toFixed(1)), parseFloat(y.toFixed(1))]);
        }
        return result;
    }

    function validatePoint(point) {
        if (!Array.isArray(point) || point.length !== 2) return false;
        return !isNaN(parseFloat(point[0])) && !isNaN(parseFloat(point[1]));
    }

    function validateZoneArray(zone) {
        if (!Array.isArray(zone)) return { valid: false, message: 'Zone harus array.' };
        if (zone.length > 0 && zone.length < 3) return { valid: false, message: 'Polygon minimal 3 titik.' };
        for (var i = 0; i < zone.length; i++) {
            if (!validatePoint(zone[i])) return { valid: false, message: 'Titik ke-' + (i + 1) + ' tidak valid.' };
        }
        return { valid: true, message: 'OK' };
    }

    function validateChannelZone(channelNo, channelData) {
        if (!channelData) return { valid: false, message: 'Channel ' + channelNo + ' tidak memiliki data.' };
        if (!channelData.image || !channelData.image.trim()) return { valid: false, message: 'Channel ' + channelNo + ' belum memiliki image path.' };
        var zv = validateZoneArray(channelData.zone || []);
        if (!zv.valid) return { valid: false, message: 'Channel ' + channelNo + ': ' + zv.message };
        if ((channelData.zone || []).length < 3) return { valid: false, message: 'Channel ' + channelNo + ' polygon minimal 3 titik.' };
        return { valid: true, message: 'OK' };
    }

    function getChannelStatus(channelData, channelKey) {
        if (!channelData) return 'EMPTY';
        var hasImage = !!(channelData.image && channelData.image.trim());
        var zone = channelData.zone || [];
        var hasZone = zone.length > 0;

        if (!hasImage && !hasZone) return 'EMPTY';
        if (!hasImage && hasZone) return 'ZONE ONLY';
        if (hasImage && !hasZone) return 'IMAGE ONLY';
        if (hasImage && hasZone && zone.length < 3) return 'IMAGE ONLY';

        var v = validateZoneArray(zone);
        if (!v.valid) return 'INVALID';
        if (vcZoningState.imageLoadedMap[channelKey] === false) return 'INVALID';
        return 'READY';
    }

    function statusClass(status) {
        if (status === 'READY') return 'status-ready';
        if (status === 'IMAGE ONLY') return 'status-image-only';
        if (status === 'ZONE ONLY') return 'status-zone-only';
        if (status === 'INVALID') return 'status-invalid';
        return 'status-empty';
    }

    function parseLegacyZonePoints(zonePointsText, activeChannels) {
        var result = {};
        if (!zonePointsText || !zonePointsText.trim()) return result;
        var parts = zonePointsText.split(';');
        for (var i = 0; i < activeChannels.length; i++) {
            var ch = activeChannels[i];
            var part = (i < parts.length) ? parts[i].trim() : '';
            var zone = [];
            if (part) { try { zone = JSON.parse(part); } catch (e) { zone = []; } }
            result[ch] = { image: '', zone: normalizeZoneArray(zone) };
        }
        return result;
    }

    function normalizeZoningData(zoningData, activeChannels) {
        var result = {};
        if (!zoningData || typeof zoningData !== 'object') return result;
        for (var i = 0; i < activeChannels.length; i++) {
            var ch = activeChannels[i];
            var src = zoningData[ch];
            if (!src) continue;
            result[ch] = {
                image: normalizeImagePath(src.image || ''),
                zone: normalizeZoneArray(src.zone)
            };
        }
        return result;
    }

    function remapZoningDataByChannelOrder(zoningData, rawChannels, activeChannels) {
        var normalized = normalizeZoningData(zoningData, activeChannels);
        if (Object.keys(normalized).length > 0) return normalized;
        var result = {};
        var rawKeys = Object.keys(zoningData || {});
        for (var i = 0; i < activeChannels.length; i++) {
            var activeCh = activeChannels[i];
            var rawCh = rawChannels[i];
            var src = (rawCh && zoningData[rawCh]) ? zoningData[rawCh] : (rawKeys[i] ? zoningData[rawKeys[i]] : null);
            if (!src) continue;
            result[activeCh] = {
                image: normalizeImagePath(src.image || ''),
                zone: normalizeZoneArray(src.zone)
            };
        }
        return result;
    }

    function parseZonePoints(zonePointsText, activeChannels, rawChannels) {
        rawChannels = rawChannels || activeChannels;
        if (!zonePointsText || !zonePointsText.trim()) return {};
        var trimmed = zonePointsText.trim();
        if (trimmed.charAt(0) === '{') {
            try {
                return remapZoningDataByChannelOrder(JSON.parse(trimmed), rawChannels, activeChannels);
            } catch (e) {
                return parseLegacyZonePoints(zonePointsText, activeChannels);
            }
        }
        return parseLegacyZonePoints(zonePointsText, activeChannels);
    }

    function validateAllZoning(zoningData, activeChannels) {
        if (!activeChannels || !activeChannels.length) return { valid: false, message: 'Channel aktif tidak ditemukan.' };
        var keys = Object.keys(zoningData || {});
        for (var k = 0; k < keys.length; k++) {
            if (activeChannels.indexOf(keys[k]) < 0) return { valid: false, message: 'Key channel "' + keys[k] + '" di luar channel aktif.' };
        }
        var hasReady = false;
        for (var i = 0; i < activeChannels.length; i++) {
            var ch = activeChannels[i];
            var data = zoningData[ch];
            if (!data) continue;
            var hasImage = data.image && data.image.trim();
            var hasZone = data.zone && data.zone.length > 0;
            if (!hasImage && !hasZone) continue;
            if (!hasImage && hasZone) {
                return { valid: false, message: 'CH ' + ch + ' memiliki zone points tetapi belum memiliki image. Upload image terlebih dahulu atau Clear Points.' };
            }
            var v = validateChannelZone(ch, data);
            if (!v.valid) return v;
            hasReady = true;
        }
        if (!hasReady) return { valid: false, message: 'Minimal satu channel harus memiliki image dan zone valid (>= 3 titik).' };
        return { valid: true, message: 'Semua data zoning valid.' };
    }

    function showPreviewError(msg) {
        $('#zoningPreviewError').text(msg || '').toggle(!!msg);
    }

    function getSelectedChannelData() {
        var ch = vcZoningState.selectedChannel;
        if (!ch) return null;
        if (!vcZoningState.zoningData[ch]) {
            vcZoningState.zoningData[ch] = { image: '', zone: [] };
        }
        if (!Array.isArray(vcZoningState.zoningData[ch].zone)) {
            vcZoningState.zoningData[ch].zone = [];
        }
        return vcZoningState.zoningData[ch];
    }

    function isPreviewImageLoadedForChannel(ch) {
        ch = ch || vcZoningState.selectedChannel;
        if (!ch) return false;
        if (vcZoningState.imageLoadedMap[ch] !== true) return false;
        var img = document.getElementById('zoningPreviewImg');
        return !!(img && img.naturalWidth > 0 && img.naturalHeight > 0 && img.style.display !== 'none');
    }

    function canDrawOnCanvas() {
        var ch = vcZoningState.selectedChannel;
        if (!ch) return false;
        var data = getSelectedChannelData();
        if (!data) return false;
        if (!data.image || !data.image.trim()) return false;
        return isPreviewImageLoadedForChannel(ch);
    }

    function updateCanvasInteractivity() {
        var canvas = document.getElementById('zoningPreviewCanvas');
        if (!canvas) return;
        if (canDrawOnCanvas()) {
            canvas.classList.remove('zoning-canvas-disabled');
            canvas.classList.add('zoning-canvas-enabled');
        } else {
            canvas.classList.remove('zoning-canvas-enabled');
            canvas.classList.add('zoning-canvas-disabled');
        }
    }

    function clearCanvasOnly() {
        var canvas = document.getElementById('zoningPreviewCanvas');
        if (!canvas) return;
        var ctx = canvas.getContext('2d');
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        canvas.width = 0;
        canvas.height = 0;
        canvas.style.width = '0px';
        canvas.style.height = '0px';
    }

    function clearPreviewImageAndCanvas(ch) {
        ch = ch || vcZoningState.selectedChannel;
        if (ch) {
            vcZoningState.imageLoadedMap[ch] = false;
        }

        var img = document.getElementById('zoningPreviewImg');
        if (img) {
            img.onload = null;
            img.onerror = null;
            img.removeAttribute('src');
            img.style.display = 'none';
        }

        clearCanvasOnly();
        showPreviewError('');
        updateCanvasInteractivity();
    }

    function updatePolygonInfo(ch) {
        ch = ch || vcZoningState.selectedChannel;
        var count = 0;
        if (ch && vcZoningState.zoningData[ch] && Array.isArray(vcZoningState.zoningData[ch].zone)) {
            count = vcZoningState.zoningData[ch].zone.length;
        }
        $('#zoningPolygonInfo').text(count + ' points');
    }

    function loadPreviewImage(imageUrl, ch) {
        ch = ch || vcZoningState.selectedChannel;
        var img = document.getElementById('zoningPreviewImg');
        if (!img) return;

        if (ch !== vcZoningState.selectedChannel) {
            return;
        }

        showPreviewError('');

        if (!imageUrl) {
            clearPreviewImageAndCanvas(ch);
            updateChannelStatus();
            return;
        }

        vcZoningState.imageLoadedMap[ch] = false;
        img.style.display = 'block';

        img.onload = function () {
            if (ch !== vcZoningState.selectedChannel) return;
            vcLog('image onload', ch, img.naturalWidth, img.naturalHeight);
            vcZoningState.imageLoadedMap[ch] = true;
            showPreviewError('');
            vcSyncCanvasSize();
            updateCanvasInteractivity();
            updateChannelStatus();
        };

        img.onerror = function () {
            if (ch !== vcZoningState.selectedChannel) return;
            vcZoningState.imageLoadedMap[ch] = false;
            clearCanvasOnly();
            showPreviewError('Preview image tidak bisa dimuat. Cek path file: ' + ($('#zoningImagePath').val() || imageUrl));
            updateCanvasInteractivity();
            updateChannelStatus();
        };

        img.src = imageUrl;
        updateCanvasInteractivity();
    }

    function loadZoningChannelPreview(ch, imagePath) {
        var localPreview = vcZoningState.previewDataUrl[ch];
        if (localPreview) {
            loadPreviewImage(localPreview, ch);
            return;
        }
        if (!imagePath) {
            loadPreviewImage('', ch);
            return;
        }
        loadPreviewImage(buildZoningImagePreviewUrl(imagePath), ch);
    }

    function syncTextareaFromPoints(ch) {
        ch = ch || vcZoningState.selectedChannel;
        var zone = [];
        if (ch && vcZoningState.zoningData[ch] && Array.isArray(vcZoningState.zoningData[ch].zone)) {
            zone = vcZoningState.zoningData[ch].zone;
        }
        $('#zoningZonePoints').val(JSON.stringify(zone, null, 2));
        updatePolygonInfo(ch);
    }

    function syncPointsFromTextarea(ch, silent) {
        ch = ch || vcZoningState.selectedChannel;
        if (!ch) return false;

        var text = $('#zoningZonePoints').val().trim();
        if (!text) {
            if (!vcZoningState.zoningData[ch]) {
                vcZoningState.zoningData[ch] = { image: ($('#zoningImagePath').val() || '').trim(), zone: [] };
            }
            vcZoningState.zoningData[ch].zone = [];
            updatePolygonInfo(ch);
            drawPolygon();
            updateChannelStatus();
            return true;
        }

        try {
            var zone = normalizeZoneArray(JSON.parse(text));
            if (!vcZoningState.zoningData[ch]) {
                vcZoningState.zoningData[ch] = { image: ($('#zoningImagePath').val() || '').trim(), zone: [] };
            }
            vcZoningState.zoningData[ch].zone = zone;
            $('#zoningZonePoints').val(JSON.stringify(zone, null, 2));
            updatePolygonInfo(ch);
            drawPolygon();
            updateChannelStatus();
            return true;
        } catch (e) {
            if (!silent) {
                vcShowAlert('error', 'JSON Invalid', 'Format Zone Points tidak valid.');
            }
            updateChannelStatus();
            return false;
        }
    }

    function drawPolygon() {
        var canvas = document.getElementById('zoningPreviewCanvas');
        var img = document.getElementById('zoningPreviewImg');
        if (!canvas || !img) return;

        var ctx = canvas.getContext('2d');
        ctx.clearRect(0, 0, canvas.width, canvas.height);

        var ch = vcZoningState.selectedChannel;
        var data = ch ? vcZoningState.zoningData[ch] : null;

        if (!canDrawOnCanvas() || !data || !data.zone || !data.zone.length || !img.naturalWidth) {
            return;
        }

        var scaleX = img.clientWidth / img.naturalWidth;
        var scaleY = img.clientHeight / img.naturalHeight;
        var points = data.zone.map(function (pt) {
            return { x: pt[0] * scaleX, y: pt[1] * scaleY };
        });

        ctx.strokeStyle = '#00ff88';
        ctx.fillStyle = 'rgba(0, 255, 136, 0.25)';
        ctx.lineWidth = 2;
        if (points.length >= 3) {
            ctx.beginPath();
            ctx.moveTo(points[0].x, points[0].y);
            for (var i = 1; i < points.length; i++) ctx.lineTo(points[i].x, points[i].y);
            ctx.closePath();
            ctx.fill();
            ctx.stroke();
        } else if (points.length >= 2) {
            ctx.beginPath();
            ctx.moveTo(points[0].x, points[0].y);
            for (var j = 1; j < points.length; j++) ctx.lineTo(points[j].x, points[j].y);
            ctx.stroke();
        }
        points.forEach(function (p, idx) {
            ctx.beginPath();
            ctx.arc(p.x, p.y, 5, 0, Math.PI * 2);
            ctx.fillStyle = idx === 0 ? '#ffcc00' : '#00ff88';
            ctx.fill();
            ctx.strokeStyle = '#003481';
            ctx.lineWidth = 1;
            ctx.stroke();
        });
    }

    function vcSyncCanvasSize() {
        var img = document.getElementById('zoningPreviewImg');
        var canvas = document.getElementById('zoningPreviewCanvas');
        if (!img || !canvas) return;

        if (!canDrawOnCanvas() || !img.clientWidth || !img.naturalWidth) {
            clearCanvasOnly();
            updateCanvasInteractivity();
            return;
        }

        canvas.width = img.clientWidth;
        canvas.height = img.clientHeight;
        canvas.style.width = img.clientWidth + 'px';
        canvas.style.height = img.clientHeight + 'px';
        drawPolygon();
        updateCanvasInteractivity();
    }

    function updateChannelStatus() {
        vcRenderChannelList();
    }

    function vcSyncCurrentChannelFromEditor() {
        var ch = vcZoningState.selectedChannel;
        if (!ch) return;
        syncPointsFromTextarea(ch, true);
        if (!vcZoningState.zoningData[ch]) vcZoningState.zoningData[ch] = { image: '', zone: [] };
        vcZoningState.zoningData[ch].image = ($('#zoningImagePath').val() || '').trim();
    }

    function saveCurrentChannelEditor() {
        vcSyncCurrentChannelFromEditor();
    }

    function vcRenderChannelList() {
        var $list = $('#zoningChannelList');
        $list.empty();
        vcZoningState.activeChannels.forEach(function (ch) {
            var data = vcZoningState.zoningData[ch] || { image: '', zone: [] };
            var status = getChannelStatus(data, ch);
            var $item = $('<a href="#" class="list-group-item"></a>')
                .attr('data-channel', ch)
                .html(formatChannelLabel(ch) + ' <span class="pull-right ' + statusClass(status) + '">' + status + '</span>');
            if (ch === vcZoningState.selectedChannel) $item.addClass('active');
            $list.append($item);
        });
    }

    function vcUpdateZoneEditorFromChannel(ch) {
        if (!vcZoningState.zoningData[ch]) {
            vcZoningState.zoningData[ch] = { image: '', zone: [] };
        }
        var data = vcZoningState.zoningData[ch];
        data.zone = normalizeZoneArray(data.zone || []);

        $('#zoningSelectedChannelLabel').text(formatChannelLabel(ch));
        $('#zoningImagePath').val(data.image || '');
        syncTextareaFromPoints(ch);
        $('#zoningImageUpload').val('');

        if (data.image && data.image.trim()) {
            loadZoningChannelPreview(ch, data.image.trim());
        } else {
            clearPreviewImageAndCanvas(ch);
        }

        updatePolygonInfo(ch);
        updateCanvasInteractivity();
        updateChannelStatus();
    }

    function vcSelectChannel(ch) {
        if (!ch) return;
        saveCurrentChannelEditor();
        vcZoningState.selectedChannel = ch;
        vcUpdateZoneEditorFromChannel(ch);
    }

    function vcBuildSavePayload() {
        vcSyncCurrentChannelFromEditor();
        var payload = {};
        vcZoningState.activeChannels.forEach(function (ch) {
            var data = vcZoningState.zoningData[ch];
            if (!data) return;
            var v = validateChannelZone(ch, data);
            if (v.valid) {
                payload[ch] = {
                    image: normalizeImagePath(data.image),
                    zone: data.zone.map(function (pt) { return [parseFloat(pt[0]), parseFloat(pt[1])]; })
                };
            }
        });
        return payload;
    }

    function vcHideOverlay() { $("#overlay").fadeOut(300); }

    function getZoningApiUrl() {
        return window.vcZoningApiUrl || window.vcZoningUploadUrl || 'vehicle_counting_zoning_api.ashx';
    }

    function isHtmlResponse(text) {
        text = text || '';
        return text.indexOf('<!DOCTYPE') >= 0 || text.indexOf('<html') >= 0;
    }

    function parseAjaxJsonResponse(responseText) {
        if (!responseText) return null;
        var text = String(responseText).trim();
        if (!text) return null;
        try {
            return JSON.parse(text);
        } catch (e1) {
            var htmlIdx = text.search(/<!DOCTYPE|<html/i);
            if (htmlIdx > 0) {
                try {
                    return JSON.parse(text.substring(0, htmlIdx).trim());
                } catch (e2) {
                    return null;
                }
            }
        }
        return null;
    }

    function getZoningAjaxErrorMessage(xhr) {
        var msg = 'Permintaan gagal.';
        if (!xhr || !xhr.responseText) return msg;
        var res = parseAjaxJsonResponse(xhr.responseText);
        if (res && res.message) return res.message;
        if (xhr.responseText.indexOf('<!DOCTYPE') >= 0 || xhr.responseText.indexOf('<html') >= 0) {
            return 'Server mengembalikan HTML, bukan JSON. Periksa handler AJAX zoning.';
        }
        return msg;
    }

    function vcZoningAjax(options) {
        var settings = $.extend({
            data: {},
            processData: true,
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            errorMessage: 'Permintaan gagal.',
            onSuccess: null,
            onComplete: null
        }, options || {});

        return $.ajax({
            url: getZoningApiUrl(),
            type: 'POST',
            cache: false,
            dataType: 'json',
            data: settings.data,
            processData: settings.processData,
            contentType: settings.contentType,
            success: function (res) {
                if (res && res.success === true) {
                    if (settings.onSuccess) settings.onSuccess(res);
                    return;
                }
                vcShowAlert('error', 'Gagal', (res && res.message) ? res.message : settings.errorMessage);
            },
            error: function (xhr) {
                var res = parseAjaxJsonResponse(xhr.responseText);
                if (res && res.success === true) {
                    if (settings.onSuccess) settings.onSuccess(res);
                    return;
                }
                if (isHtmlResponse(xhr.responseText)) {
                    vcShowAlert('error', 'Gagal', 'Server mengembalikan HTML, bukan JSON. Periksa handler AJAX zoning.');
                    return;
                }
                vcShowAlert('error', 'Gagal', getZoningAjaxErrorMessage(xhr));
            },
            complete: function () {
                if (settings.onComplete) settings.onComplete();
            }
        });
    }

    function getZoningById(id) {
        postZoning(id);
    }

    function openZoningModalFromData(d) {
        vcZoningState.id = d.id;
        vcZoningState.companyName = d.company_name || '';
        vcZoningState.vehicleId = d.vehicle_id || '';
        vcZoningState.nopol = d.nopol || '';
        vcZoningState.gpsSn = d.gps_sn || '';
        vcZoningState.channelRaw = d.channel || '';
        vcZoningState.channelText = d.next_channel || d.channel || '';
        vcZoningState.activeChannels = parseChannelList(vcZoningState.channelText);
        vcZoningState.imageLoadedMap = {};
        vcZoningState.previewDataUrl = {};
        vcZoningState.zoningData = parseZonePoints(
            d.zone_points || '',
            vcZoningState.activeChannels,
            parseChannelList(vcZoningState.channelRaw)
        );

        if (!vcZoningState.activeChannels.length) {
            vcShowAlert('warning', 'Perhatian', 'Channel aktif tidak ditemukan.');
            return;
        }

        $('#zoningInfoCompany').text(vcZoningState.companyName);
        $('#zoningInfoVehicleId').text(vcZoningState.vehicleId);
        $('#zoningInfoNopol').text(vcZoningState.nopol);
        $('#zoningInfoGpsSn').text(vcZoningState.gpsSn);
        $('#zoningInfoChannel').text(vcZoningState.channelText);

        vcSelectChannel(vcZoningState.activeChannels[0]);
        $('#modal-zoning').modal('show');
    }

    function postZoning(id) {
        if (!id) {
            vcShowAlert('error', 'Gagal', 'ID Vehicle Counting tidak ditemukan.');
            return;
        }

        showOverlay();
        vcZoningAjax({
            data: { action: 'get_zoning', id: id },
            errorMessage: 'Gagal mengambil data zoning.',
            onSuccess: function (res) {
                openZoningModalFromData(res.data || {});
            },
            onComplete: function () {
                vcHideOverlay();
            }
        });
    }

    function handleUploadImageSuccess(res, ch) {
        if (ch !== vcZoningState.selectedChannel) return;

        var imagePath = res.image_path || (res.data && res.data.image) || '';
        var imageUrl = res.image_url || (res.data && res.data.image_url) || buildZoningImagePreviewUrl(imagePath);
        if (!vcZoningState.zoningData[ch]) vcZoningState.zoningData[ch] = { image: '', zone: [] };
        vcZoningState.zoningData[ch].image = imagePath;
        if (!vcZoningState.zoningData[ch].zone) vcZoningState.zoningData[ch].zone = [];

        $('#zoningImagePath').val(imagePath);
        syncTextareaFromPoints(ch);

        var previewUrl = imageUrl + (imageUrl.indexOf('?') >= 0 ? '&' : '?') + 'v=' + new Date().getTime();
        if (vcZoningState.previewDataUrl[ch]) {
            loadPreviewImage(vcZoningState.previewDataUrl[ch], ch);
        } else {
            loadPreviewImage(previewUrl, ch);
        }

        updateChannelStatus();
        vcShowAlert('success', 'Berhasil', res.message || 'Image berhasil diupload.');
    }

    function addPointToSelectedChannel(point) {
        var ch = vcZoningState.selectedChannel;
        if (!ch) return;

        if (!canDrawOnCanvas()) {
            vcShowAlert('warning', 'Perhatian', 'Upload image terlebih dahulu sebelum membuat polygon.');
            return;
        }

        var data = getSelectedChannelData();
        data.image = ($('#zoningImagePath').val() || data.image || '').trim();
        data.zone.push([parseFloat(point[0].toFixed(1)), parseFloat(point[1].toFixed(1))]);

        syncTextareaFromPoints(ch);
        drawPolygon();
        updateChannelStatus();
    }

    function handlePolygonClick(e, element) {
        if (!canDrawOnCanvas()) {
            vcShowAlert('warning', 'Perhatian', 'Upload image terlebih dahulu sebelum membuat polygon.');
            return;
        }

        var ch = vcZoningState.selectedChannel;
        var img = document.getElementById('zoningPreviewImg');
        if (!ch || !img || !img.naturalWidth) {
            vcShowAlert('warning', 'Perhatian', 'Upload image terlebih dahulu sebelum membuat polygon.');
            return;
        }

        var rect = element.getBoundingClientRect();
        var xDisplay = e.clientX - rect.left;
        var yDisplay = e.clientY - rect.top;
        var xOrig = xDisplay * (img.naturalWidth / img.clientWidth);
        var yOrig = yDisplay * (img.naturalHeight / img.clientHeight);

        vcLog('click display/original', xDisplay, yDisplay, xOrig, yOrig);
        addPointToSelectedChannel([xOrig, yOrig]);
    }

    function handleZoningImageUpload(fileInput) {
        var file = fileInput.files && fileInput.files[0];
        var ch = vcZoningState.selectedChannel;
        if (!file || !ch) return;
        if (!vcZoningState.gpsSn) { vcShowAlert('error', 'Error', 'GPS SN tidak tersedia.'); return; }

        var ext = (file.name.split('.').pop() || '').toLowerCase();
        if (['jpg', 'jpeg', 'png'].indexOf(ext) < 0) {
            vcShowAlert('error', 'Validasi', 'Format file hanya jpg, jpeg, atau png.');
            $(fileInput).val('');
            return;
        }
        if (file.size > 5 * 1024 * 1024) {
            vcShowAlert('error', 'Validasi', 'Ukuran file maksimal 5MB.');
            $(fileInput).val('');
            return;
        }

        vcLog('upload start', file.name, ch, vcZoningState.id, vcZoningState.gpsSn);

        var reader = new FileReader();
        reader.onload = function (e) {
            vcZoningState.previewDataUrl[ch] = e.target.result;
            loadPreviewImage(e.target.result, ch);
        };
        reader.readAsDataURL(file);

        var formData = new FormData();
        formData.append('action', 'upload_zoning_image');
        formData.append('id', vcZoningState.id);
        formData.append('gps_sn', vcZoningState.gpsSn);
        formData.append('channel_no', ch);
        formData.append('file', file);

        showOverlay();
        vcZoningAjax({
            data: formData,
            processData: false,
            contentType: false,
            errorMessage: 'Upload image gagal.',
            onSuccess: function (res) {
                vcLog('upload response', res);
                handleUploadImageSuccess(res, ch);
            },
            onComplete: function () {
                vcHideOverlay();
            }
        });
    }

    function bindZoningEvents() {
        if (window._vcZoningEventsBound) {
            return;
        }
        window._vcZoningEventsBound = true;

        var $doc = $(document);

        $doc.off('click.vcZoning', '.btn-zoning').on('click.vcZoning', '.btn-zoning', function (e) {
            e.preventDefault();
            e.stopPropagation();
            e.stopImmediatePropagation();
            var id = $(this).attr('data-id') || $(this).data('id');
            postZoning(id);
            return false;
        });

        $doc.off('click.vcZoning', '#zoningChannelList .list-group-item').on('click.vcZoning', '#zoningChannelList .list-group-item', function (e) {
            e.preventDefault();
            vcSelectChannel($(this).attr('data-channel'));
        });

        $doc.off('click.vcZoning', '#zoningPreviewCanvas').on('click.vcZoning', '#zoningPreviewCanvas', function (e) {
            e.preventDefault();
            e.stopPropagation();
            handlePolygonClick(e, this);
        });

        $doc.off('click.vcZoning', '#zoningPreviewImg').on('click.vcZoning', '#zoningPreviewImg', function (e) {
            if (!canDrawOnCanvas()) return;
            e.preventDefault();
            e.stopPropagation();
            handlePolygonClick(e, this);
        });

        $doc.off('change blur.vcZoning', '#zoningZonePoints').on('change blur.vcZoning', '#zoningZonePoints', function () {
            var ch = vcZoningState.selectedChannel;
            if (ch) syncPointsFromTextarea(ch, true);
        });

        $doc.off('click.vcZoning', '#btnZoningUndo').on('click.vcZoning', '#btnZoningUndo', function () {
            var ch = vcZoningState.selectedChannel;
            if (!ch || !vcZoningState.zoningData[ch] || !vcZoningState.zoningData[ch].zone.length) return;
            vcZoningState.zoningData[ch].zone.pop();
            syncTextareaFromPoints(ch);
            drawPolygon();
            updateChannelStatus();
        });

        $doc.off('click.vcZoning', '#btnZoningClearPoints').on('click.vcZoning', '#btnZoningClearPoints', function () {
            var ch = vcZoningState.selectedChannel;
            if (!ch) return;
            if (!vcZoningState.zoningData[ch]) vcZoningState.zoningData[ch] = { image: $('#zoningImagePath').val() || '', zone: [] };
            vcZoningState.zoningData[ch].zone = [];
            syncTextareaFromPoints(ch);
            drawPolygon();
            updateChannelStatus();
        });

        $doc.off('click.vcZoning', '#btnZoningClearChannel').on('click.vcZoning', '#btnZoningClearChannel', function () {
            var ch = vcZoningState.selectedChannel;
            if (!ch) return;
            vcZoningState.zoningData[ch] = { image: '', zone: [] };
            vcZoningState.previewDataUrl[ch] = '';
            vcZoningState.imageLoadedMap[ch] = false;
            vcUpdateZoneEditorFromChannel(ch);
        });

        $doc.off('click.vcZoning', '#btnZoningValidate').on('click.vcZoning', '#btnZoningValidate', function () {
            vcSyncCurrentChannelFromEditor();
            var ch = vcZoningState.selectedChannel;
            if (!ch) { vcShowAlert('warning', 'Perhatian', 'Pilih channel terlebih dahulu.'); return; }
            var v = validateChannelZone(ch, vcZoningState.zoningData[ch]);
            if (v.valid) vcShowAlert('success', 'Valid', 'Channel ' + formatChannelLabel(ch) + ' valid.');
            else vcShowAlert('error', 'Tidak Valid', v.message);
            updateChannelStatus();
        });

        $doc.off('click.vcZoning', '#btnZoningImportJson').on('click.vcZoning', '#btnZoningImportJson', function () {
            Swal.fire({
                title: 'Import JSON per Channel',
                input: 'textarea',
                inputPlaceholder: '{ "image": "Picture/zoning/.../channel_5.jpg", "zone": [[x,y],...] }',
                showCancelButton: true,
                confirmButtonText: 'Import',
                confirmButtonColor: '#003481'
            }).then(function (result) {
                if (!result.isConfirmed || !result.value) return;
                var ch = vcZoningState.selectedChannel;
                if (!ch) return;
                try {
                    var parsed = JSON.parse(result.value.trim());
                    var zone = normalizeZoneArray(parsed.zone);
                    if (zone.length < 3) { vcShowAlert('error', 'Validasi', 'Zone minimal 3 titik.'); return; }
                    var image = normalizeImagePath(parsed.image || '');
                    vcZoningState.zoningData[ch] = { image: image, zone: zone };
                    vcZoningState.previewDataUrl[ch] = '';
                    vcUpdateZoneEditorFromChannel(ch);
                    vcShowAlert('success', 'Berhasil', 'JSON channel berhasil diimport.');
                } catch (e) {
                    vcShowAlert('error', 'JSON Invalid', 'Format JSON tidak valid.');
                }
            });
        });

        $doc.off('change.vcZoning', '#zoningImageUpload').on('change.vcZoning', '#zoningImageUpload', function () {
            handleZoningImageUpload(this);
        });

        $doc.off('click.vcZoning', '#btnZoningSave').on('click.vcZoning', '#btnZoningSave', function () {
            saveCurrentChannelEditor();
            var preCheck = validateAllZoning(vcZoningState.zoningData, vcZoningState.activeChannels);
            if (!preCheck.valid) {
                vcShowAlert('error', 'Validasi', preCheck.message);
                return;
            }
            var payload = vcBuildSavePayload();
            var v = validateAllZoning(payload, vcZoningState.activeChannels);
            if (!v.valid) { vcShowAlert('error', 'Validasi', v.message); return; }
            var json = JSON.stringify(payload);
            vcLog('save zoning', json);
            showOverlay();
            vcZoningAjax({
                data: {
                    action: 'save_zoning',
                    id: vcZoningState.id,
                    zone_points: json
                },
                errorMessage: 'Gagal menyimpan zoning.',
                onSuccess: function (res) {
                    vcZoningState.zoningData = parseZonePoints(json, vcZoningState.activeChannels, parseChannelList(vcZoningState.channelRaw));
                    vcZoningState.previewDataUrl = {};
                    updateChannelStatus();
                    vcShowAlert('success', 'Berhasil', res.message || 'Zoning berhasil disimpan.');
                },
                onComplete: function () {
                    vcHideOverlay();
                }
            });
        });

        $(window).off('resize.vcZoning').on('resize.vcZoning', function () {
            if ($('#modal-zoning').is(':visible')) vcSyncCanvasSize();
        });

        $('#modal-zoning').off('shown.bs.modal.vcZoning').on('shown.bs.modal.vcZoning', function () {
            updateCanvasInteractivity();
            vcSyncCanvasSize();
        });
    }

    window.postZoning = postZoning;
    window.getZoningById = getZoningById;
    window.vcZoningInit = function () {
        bindZoningEvents();
    };

    $(document).ready(function () {
        if (window.vcZoningInit) window.vcZoningInit();
    });

})(window, jQuery);
