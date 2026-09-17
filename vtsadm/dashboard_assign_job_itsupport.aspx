<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="dashboard_assign_job_itsupport.aspx.cs" Inherits="vtsadm.dashboard_assign_job_itsupport" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .assign-dashboard {
            padding-bottom: 20px;
            color: #1a1a18;
            overflow-x: hidden;
        }

        .assign-dashboard,
        .assign-dashboard * {
            box-sizing: border-box;
        }

        .assign-topbar {
            position: sticky;
            top: 0;
            z-index: 99;
            background: #ffffff;
            border: 1px solid #e5e3df;
            border-radius: 12px;
            min-height: 60px;
            padding: 0 18px;
            margin-bottom: 16px;
            display: flex;
            align-items: center;
            justify-content: flex-start;
            gap: 14px;
            flex-wrap: nowrap;
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.08);
        }

        .assign-brand {
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .assign-brand-icon {
            width: 32px;
            height: 32px;
            border-radius: 8px;
            background: #0a5c48;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            color: #fff;
            font-size: 14px;
            font-weight: 700;
            flex-shrink: 0;
        }

        .assign-brand-name {
            font-size: 15px;
            font-weight: 600;
            line-height: 1.1;
            letter-spacing: -.2px;
        }

        .assign-brand-sub {
            font-size: 12px;
            color: #9b9b93;
        }

        .assign-topbar-divider {
            width: 1px;
            height: 28px;
            background: #e5e3df;
            flex-shrink: 0;
        }

        .assign-topbar-right {
            margin-left: auto;
            display: flex;
            align-items: center;
            gap: 8px;
            flex-wrap: wrap;
        }

        .assign-period {
            width: 110px;
            height: 30px;
            border-radius: 7px;
            border: 1px solid #d1cfc9;
            box-shadow: none;
            font-size: 12px;
            padding: 4px 8px;
        }

        .assign-btn {
            border-radius: 7px;
            min-width: 74px;
            font-size: 12px;
            padding: 5px 10px;
            font-weight: 600;
        }

        .assign-create-jo-btn {
            min-width: 0;
            white-space: nowrap;
            background: #0a5c48;
            border-color: #0a5c48;
            color: #fff;
        }

        .assign-create-jo-btn:hover,
        .assign-create-jo-btn:focus {
            background: #084c3b;
            border-color: #084c3b;
            color: #fff;
        }

        .assign-create-jo-modal {
            max-width: 860px;
        }

        #assignCreateJoBackdrop {
            z-index: 1200;
        }

        .assign-create-jo-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 16px;
        }

        .assign-create-jo-col {
            min-width: 0;
        }

        .assign-create-jo-col .assign-field {
            margin-bottom: 10px;
        }

        body.assign-create-jo-open .modal-backdrop {
            z-index: 1350;
        }

        body.assign-create-jo-open #modal-training-customer {
            z-index: 1400;
        }

        @media (max-width: 768px) {
            .assign-create-jo-grid {
                grid-template-columns: 1fr;
            }
        }

        .assign-tabs {
            display: inline-flex;
            background: #f8f7f5;
            border: 1px solid #e5e3df;
            border-radius: 7px;
            padding: 3px;
            gap: 2px;
        }

        .assign-tab-btn {
            border: 0;
            background: transparent;
            color: #5c5c55;
            font-weight: 500;
            font-size: 12px;
            padding: 5px 12px;
            border-radius: 5px;
            transition: all .2s ease;
            min-height: 34px;
            touch-action: manipulation;
        }

        .assign-tab-btn.active {
            background: #ffffff;
            color: #0a5c48;
            box-shadow: 0 1px 3px rgba(0, 0, 0, .08);
        }

        .summary-grid {
            display: grid;
            grid-template-columns: repeat(4, 1fr);
            gap: 12px;
            margin-bottom: 14px;
        }

        .summary-card {
            background: #fff;
            border: 1px solid #e5e3df;
            border-radius: 14px;
            box-shadow: 0 1px 3px rgba(0, 0, 0, .08);
            padding: 16px 18px;
            cursor: pointer;
            position: relative;
            transition: transform .2s ease, box-shadow .2s ease;
            overflow: hidden;
        }

        .summary-card:hover {
            transform: translateY(-2px);
            box-shadow: 0 8px 18px rgba(0, 0, 0, 0.1);
        }

        .summary-card:before {
            content: "";
            height: 3px;
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
        }

        .card-total:before { background: #0a5c48; }
        .card-open:before { background: #f39c12; }
        .card-scheduled:before { background: #2563eb; }
        .card-close:before { background: #16a34a; }

        .assign-summary-title {
            margin: 0 0 7px;
            color: #9b9b93;
            font-size: 11px;
            text-transform: uppercase;
            letter-spacing: .6px;
            font-weight: 700;
        }

        .summary-split-group {
            border-top: 1px solid #e5e3df;
            padding-top: 8px;
            margin-top: 9px;
        }

        .summary-split-caption {
            margin: 0 0 6px;
            font-size: 11px;
            font-weight: 700;
            color: #7b7b74;
            text-transform: uppercase;
            letter-spacing: .35px;
        }

        .summary-split-row {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 8px;
            font-size: 12px;
            line-height: 1.45;
            margin-bottom: 3px;
        }

        .summary-split-row:last-child {
            margin-bottom: 0;
        }

        .summary-split-row .label {
            color: #8f8f89;
            font-weight: 600;
        }

        .summary-split-row .value {
            color: #1f1f1d;
            font-weight: 700;
            font-size: 15px;
        }

        .summary-column-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 10px;
            margin-top: 7px;
        }

        .summary-column {
            border: 1px solid #eceae6;
            border-radius: 10px;
            padding: 8px;
            background: #fcfcfb;
        }

        .summary-column-title {
            margin: 0 0 6px;
            font-size: 11px;
            font-weight: 700;
            color: #6f6f68;
            text-transform: uppercase;
            letter-spacing: .3px;
        }

        .summary-link-value {
            border: 0;
            background: transparent;
            padding: 0;
            margin: 0;
            color: #0a5c48;
            font-size: 22px;
            font-weight: 800;
            line-height: 1;
            cursor: pointer;
        }

        .summary-link-value:hover {
            text-decoration: underline;
        }

        .summary-unit-line {
            margin-top: 8px;
            border-top: 1px solid #e5e3df;
            padding-top: 6px;
            display: flex;
            align-items: baseline;
            justify-content: space-between;
            gap: 8px;
        }

        .summary-unit-label {
            font-size: 11px;
            color: #909088;
            text-transform: uppercase;
            letter-spacing: .25px;
        }

        .summary-unit-value {
            color: #1f1f1d;
            font-size: 16px;
            font-weight: 700;
        }

        .section-box {
            background: #fff;
            border: 1px solid #e5e3df;
            border-radius: 14px;
            box-shadow: 0 1px 3px rgba(0, 0, 0, .08);
            margin-bottom: 14px;
            overflow: hidden;
        }

        .section-header {
            padding: 12px 16px;
            border-bottom: 1px solid #e5e3df;
            background: #fafaf8;
            display: flex;
            align-items: center;
            gap: 8px;
            flex-wrap: wrap;
        }

        .section-title {
            margin: 0;
            font-size: 13px;
            font-weight: 700;
            color: #1a1a18;
        }

        .panel-badge {
            font-size: 11px;
            background: #e4f4ef;
            color: #0a5c48;
            padding: 2px 8px;
            border-radius: 20px;
            font-weight: 600;
        }

        .area-filter-bar {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            margin-left: auto;
            flex-wrap: wrap;
            max-width: 100%;
        }

        .area-filter-wrap {
            margin-left: auto;
            display: flex;
            flex-direction: column;
            align-items: flex-end;
            gap: 6px;
            max-width: 100%;
        }

        .area-group-filter-bar {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            flex-wrap: wrap;
            max-width: 100%;
        }

        .area-filter-btn {
            border: 1px solid #d1cfc9;
            border-radius: 20px;
            padding: 4px 10px;
            font-size: 11px;
            font-weight: 600;
            color: #5c5c55;
            background: #fff;
            text-decoration: none !important;
            line-height: 1.2;
            display: inline-block;
            min-height: 30px;
            touch-action: manipulation;
        }

        .area-filter-btn:hover,
        .area-filter-btn:focus {
            color: #0a5c48;
            border-color: #0f8065;
        }

        .area-filter-btn.active {
            background: #0a5c48;
            color: #fff !important;
            border-color: #0a5c48;
        }

        .performance-wrap {
            padding: 14px 16px;
        }

        .perf-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(210px, 1fr));
            gap: 10px;
        }

        .perf-item {
            border: 1px solid #e5e3df;
            border-radius: 10px;
            background: #f8f7f5;
            padding: 10px 12px;
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .perf-rank {
            width: 30px;
            text-align: center;
            font-weight: 700;
            font-size: 15px;
            color: #9b9b93;
        }

        .perf-info {
            flex: 1;
            min-width: 0;
        }

        .perf-name {
            color: #1a1a18;
            font-weight: 600;
            font-size: 12px;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .perf-area {
            font-size: 10px;
            color: #9b9b93;
            margin-top: 1px;
            text-transform: uppercase;
        }

        .perf-score {
            text-align: right;
            font-weight: 700;
            font-size: 18px;
            color: #0a5c48;
        }

        .perf-score small {
            display: block;
            font-size: 10px;
            color: #9b9b93;
            margin-top: -1px;
        }

        .schedule-wrap {
            padding: 0;
            min-width: 0;
        }

        .assign-table-wrap {
            border-top: 0;
            overflow-x: auto;
            overflow-y: auto;
            -webkit-overflow-scrolling: touch;
            overscroll-behavior-x: contain;
            max-height: none;
            max-width: 100%;
        }

        .assign-table {
            border-collapse: separate;
            border-spacing: 0;
            width: 100%;
            min-width: 100%;
            table-layout: fixed;
            margin-bottom: 0;
        }

        .assign-table thead th {
            position: sticky;
            top: 0;
            background: #fafaf8;
            color: #9b9b93;
            font-size: 11px;
            font-weight: 700;
            border-bottom: 1px solid #e5e3df;
            z-index: 1;
            text-align: center;
            padding: 4px 4px;
            white-space: nowrap;
        }

        .assign-table thead th.col-name {
            text-align: left;
            padding-left: 10px;
        }

        .assign-table thead th.day-weekend-th {
            background: #fee2e2;
            color: #991b1b;
        }

        .assign-table thead th.day-today-th {
            background: #dbeafe;
            color: #1e3a8a;
        }

        .assign-table thead th.day-weekend-th.day-today-th {
            background: #dbeafe;
            color: #1e3a8a;
        }

        .assign-day-no {
            display: block;
            font-size: 11px;
            font-weight: 700;
            line-height: 1.1;
        }

        .assign-day-name {
            display: block;
            margin-top: 2px;
            font-size: 9px;
            font-weight: 600;
            color: #9b9b93;
            text-transform: uppercase;
            line-height: 1.1;
        }

        .assign-day-total-btn {
            display: block;
            width: 100%;
            min-width: 34px;
            margin: 0 0 4px;
            padding: 4px 2px;
            border: 0;
            border-radius: 5px;
            background: #0a5c48;
            color: #fff;
            font-size: 12px;
            font-weight: 700;
            line-height: 1.2;
            white-space: nowrap;
            text-align: center;
            cursor: pointer;
        }

        .assign-day-total-btn:hover {
            background: #0f8065;
        }

        .assign-day-total-btn.is-zero {
            background: #e5e3df;
            color: #9b9b93;
        }

        .assign-day-total-btn.is-zero:hover {
            background: #d7d4cf;
            color: #6b7280;
        }

        .assign-table thead th.col-day {
            padding: 4px 3px 5px;
            vertical-align: bottom;
        }

        .assign-table tbody td {
            border-bottom: 1px solid #e5e3df;
            border-right: 1px solid #e5e3df;
            color: #1a1a18;
            font-size: 12px;
            padding: 0;
            height: 34px;
            text-align: center;
            white-space: nowrap;
        }

        .assign-table tbody tr:nth-child(even) {
            background: #fcfcfb;
        }

        .col-no { width: 36px; min-width: 36px; }
        .col-name { width: 200px; min-width: 200px; }
        .col-total-assign { width: 72px; min-width: 72px; }
        .col-total { width: 72px; min-width: 72px; }
        .col-total-maint { width: 72px; min-width: 72px; }
        .col-stock { width: 110px; min-width: 110px; }
        .col-day { width: 42px; min-width: 42px; }

        .sticky-no, .sticky-name {
            position: -webkit-sticky;
            position: sticky;
            z-index: 2;
            background: #fff;
        }

        .sticky-no { left: 0; }
        .sticky-name { left: 36px; }

        .assign-table td.sticky-no,
        .assign-table td.sticky-name {
            box-shadow: 1px 0 0 #e5e3df;
        }

        .assign-table td.sticky-no,
        .assign-table th.sticky-no {
            z-index: 8;
        }

        .assign-table td.sticky-name,
        .assign-table th.sticky-name {
            z-index: 7;
            box-shadow: 2px 0 0 #d7d4cf;
        }

        .assign-table thead th.col-total-assign,
        .assign-table thead th.col-total,
        .assign-table thead th.col-total-maint {
            font-size: 10px;
            white-space: normal;
            line-height: 1.2;
            padding: 4px 2px;
        }

        .assign-table th.sticky-no, .assign-table th.sticky-name {
            background: #fafaf8;
            z-index: 3;
        }

        /* Keep No & Nama frozen during horizontal scroll */
        .assign-table th.col-no,
        .assign-table td.col-no {
            position: -webkit-sticky !important;
            position: sticky !important;
            left: 0 !important;
            z-index: 11 !important;
            background: #fff;
        }

        .assign-table th.col-name,
        .assign-table td.col-name {
            position: -webkit-sticky !important;
            position: sticky !important;
            left: 36px !important;
            z-index: 10 !important;
            background: #fff;
        }

        .assign-table thead th.col-no,
        .assign-table thead th.col-name {
            background: #fafaf8;
        }

        .cell-no {
            font-size: 11px;
            color: #9b9b93;
            font-weight: 700;
        }

        .assign-table tbody td.cell-name {
            text-align: left;
            padding: 0 10px;
            font-weight: 600;
        }

        .cell-total,
        .cell-stock {
            padding: 0 4px;
        }

        .assign-totaljob-link {
            border: 0;
            background: transparent;
            color: #1d4ed8;
            font-size: 12px;
            font-weight: 600;
            text-decoration: underline;
            cursor: pointer;
            line-height: 1.1;
            padding: 0;
        }

        .assign-totaljob-link:hover {
            color: #0a5c48;
        }

        .assign-stock-btn {
            border: 1px solid #b2ddd2;
            background: #e4f4ef;
            color: #0a5c48;
            border-radius: 10px;
            min-width: 78px;
            height: 28px;
            font-size: 11px;
            font-weight: 700;
            padding: 0 10px;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            gap: 6px;
            cursor: pointer;
        }

        .assign-stock-btn:hover {
            background: #0f8065;
            color: #fff;
        }

        .assign-stock-icon {
            font-size: 12px;
            line-height: 1;
        }

        .cell-area {
            font-size: 11px;
            font-weight: 700;
            color: #0a5c48;
        }

        .status-cell {
            width: 100%;
            height: 100%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 12px;
            font-weight: 700;
            border-radius: 8px;
            padding: 2px 4px;
            line-height: 1;
            min-height: 30px;
        }

        .day-weekend-cell {
            background: #fee2e2;
        }

        .day-today-cell {
            background: transparent;
        }

        .day-weekend-cell.day-today-cell {
            background: transparent;
        }

        .status-cell.st-av.day-today-status {
            background: #dbeafe !important;
            color: #1e3a8a !important;
            font-weight: 700;
        }

        .st-av {
            background: #e4f4ef;
            color: #0a5c48;
        }

        /* Sel angka: RemainingJo <> 0 - merah pastel (selaras AV/OFF) */
        .st-unit,
        .st-unit-open,
        .status-cell.st-unit,
        .status-cell.st-unit-open {
            background: #fee2e2 !important;
            color: #991b1b !important;
        }

        /* Sel angka: RemainingJo = 0 - biru #007bff (beda AV hari ini #dbeafe) */
        .st-unit-done,
        .status-cell.st-unit-done {
            background: #cce7ff !important;
            color: #007bff !important;
        }

        /* Cegah class AV hari ini menimpa sel angka (jika terpasang oleh kesalahan JS) */
        .status-cell.st-unit-open.day-today-status {
            background: #fee2e2 !important;
            color: #991b1b !important;
        }

        .status-cell.st-unit-done.day-today-status {
            background: #cce7ff !important;
            color: #007bff !important;
        }

        .st-close {
            background: #dcfce7;
            color: #166534;
        }

        .st-off {
            background: #f1f0ec;
            color: #6b7280;
        }

        .st-cuti {
            background: #fee2e2;
            color: #991b1b;
        }

        .st-sakit {
            background: #f5f3ff;
            color: #6d28d9;
        }

        .st-izin {
            background: #fff7ed;
            color: #9a3412;
        }

        .st-unavailable {
            background: #fecaca;
            color: #991b1b;
        }

        .legend {
            display: flex;
            flex-wrap: wrap;
            gap: 10px;
            padding: 12px 16px;
            border-top: 1px solid #e5e3df;
            background: #fafaf8;
        }

        .cap-header-cell {
            background: #0a5c48 !important;
            color: #fff !important;
            font-size: 11px !important;
            font-weight: 700 !important;
            text-align: left !important;
            padding: 6px 10px !important;
        }

        .cap-scroll-gap {
            padding: 0 !important;
            border-left: 0 !important;
        }

        .cap-row-label {
            text-align: left;
            padding: 0 10px !important;
            font-size: 11px;
            font-weight: 600;
        }

        .cap-avail { background: #ecfdf5; color: #065f46; }
        .cap-jo1 { background: #fff7d6; color: #92400e; }
        .cap-jo2 { background: #fef3c7; color: #78350f; }
        .cap-jo3 { background: #fef3c7; color: #78350f; }
        .cap-lembur { background: #f5f3ff; color: #5b21b6; }
        .cap-off { background: #f1f0ec; color: #4b5563; }
        .cap-cuti { background: #fee2e2; color: #991b1b; }
        .cap-izin { background: #fff7ed; color: #9a3412; }

        .assign-table tbody:last-child td.cap-header-cell {
            background: #0a5c48 !important;
            color: #fff !important;
        }

        .assign-table tbody:last-child td.cap-avail {
            background: #ecfdf5 !important;
            color: #065f46;
        }

        .assign-table tbody:last-child td.cap-jo1 {
            background: #fff7d6 !important;
            color: #92400e;
        }

        .assign-table tbody:last-child td.cap-jo2,
        .assign-table tbody:last-child td.cap-jo3 {
            background: #fef3c7 !important;
            color: #78350f;
        }

        .assign-table tbody:last-child td.cap-lembur {
            background: #f5f3ff !important;
            color: #5b21b6;
        }

        .assign-table tbody:last-child td.cap-off {
            background: #f1f0ec !important;
            color: #4b5563;
        }

        .assign-table tbody:last-child td.cap-cuti {
            background: #fee2e2 !important;
            color: #991b1b;
        }

        .assign-table tbody:last-child td.cap-izin {
            background: #fff7ed !important;
            color: #9a3412;
        }

        .legend-item {
            display: flex;
            align-items: center;
            gap: 6px;
            font-size: 11px;
            color: #5c5c55;
        }

        .legend-dot {
            width: 24px;
            height: 18px;
            border-radius: 5px;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            font-size: 10px;
            font-weight: 700;
        }

        .legend-dot.st-av.day-today-status {
            background: #dbeafe;
            color: #1e3a8a;
        }

        .legend-title {
            flex-basis: 100%;
            font-size: 11px;
            font-weight: 700;
            color: #374151;
        }

        .legend-note {
            flex-basis: 100%;
            font-size: 11px;
            color: #6b7280;
            line-height: 1.45;
        }

        .regional-list {
            margin: 0;
            padding: 14px 16px;
            list-style: none;
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
            gap: 10px;
        }

        .regional-item {
            border: 1px solid #e8edf4;
            border-radius: 10px;
            background: #f9fbfe;
            padding: 9px 11px;
            color: #1a1a18;
            font-weight: 600;
            font-size: 12px;
        }

        .empty-text {
            color: #8391a2;
            font-style: italic;
            padding: 10px 0;
        }

        .assign-hidden-trigger {
            display: none !important;
        }

        .assign-hidden {
            display: none !important;
        }

        .assign-cell-trigger {
            transition: transform .15s ease, filter .15s ease, box-shadow .15s ease, opacity .15s ease;
        }

        .assign-cell--clickable {
            cursor: pointer;
            box-shadow: inset 0 0 0 1px rgba(10, 92, 72, .18);
        }

        .assign-cell--clickable:hover {
            transform: none;
            filter: none;
            z-index: auto;
            position: static;
        }

        .assign-cell--clickable:active,
        .assign-cell--clickable:focus {
            transform: none;
            filter: none;
        }

        .assign-cell--report-clickable {
            cursor: pointer;
            box-shadow: inset 0 0 0 1px rgba(146, 64, 14, .18);
        }

        .assign-cell--report-clickable.st-unit-open {
            box-shadow: inset 0 0 0 1px rgba(153, 27, 27, .22) !important;
        }

        .assign-cell--report-clickable.st-unit-done {
            box-shadow: inset 0 0 0 1px rgba(0, 123, 255, .28) !important;
        }

        .assign-cell--report-clickable.st-unit-open:hover,
        .assign-cell--report-clickable.st-unit-open:active,
        .assign-cell--report-clickable.st-unit-open:focus {
            background: #fecaca !important;
            color: #991b1b !important;
            transform: none;
            filter: none;
            z-index: auto;
            position: static;
        }

        .assign-cell--report-clickable.st-unit-done:hover,
        .assign-cell--report-clickable.st-unit-done:active,
        .assign-cell--report-clickable.st-unit-done:focus {
            background: #99ccff !important;
            color: #007bff !important;
            transform: none;
            filter: none;
            z-index: auto;
            position: static;
        }

        .assign-cell--report-clickable:hover {
            transform: none;
            filter: none;
            z-index: auto;
            position: static;
        }

        .assign-cell--report-clickable:active,
        .assign-cell--report-clickable:focus {
            transform: none;
            filter: none;
        }

        .assign-cell--disabled {
            cursor: default;
        }

        .assign-cell--disabled.st-av {
            opacity: .55;
        }

        .assign-cell--clickable.st-off,
        .assign-cell--clickable.st-unavailable {
            box-shadow: none;
        }

        .detail-jo-dialog {
            width: 96vw;
            max-width: 1280px;
        }

        .detail-jo-content {
            border-radius: 14px;
            overflow: hidden;
            border: 1px solid #e5e7eb;
            box-shadow: 0 18px 42px rgba(0, 0, 0, .22);
            display: flex;
            flex-direction: column;
            max-height: 90vh;
        }

        .detail-jo-body {
            overflow: auto;
            padding: 14px 16px 10px;
            flex: 1;
            min-height: 0;
        }

        .detail-jo-search-wrap {
            display: flex;
            justify-content: flex-end;
            margin-bottom: 10px;
        }

        .detail-jo-search-input {
            width: min(320px, 100%);
            border: 1px solid #d6d3cf;
            border-radius: 8px;
            padding: 7px 10px;
            font-size: 12px;
            color: #1f2937;
            background: #fff;
        }

        .detail-jo-search-input:focus {
            outline: none;
            border-color: #2563eb;
            box-shadow: 0 0 0 2px rgba(37, 99, 235, .12);
        }

        .detail-jo-search-empty td {
            text-align: center;
            color: #6b7280;
            font-size: 12px;
            font-style: italic;
            padding: 12px 10px;
            background: #fafafa;
        }

        .detail-jo-table-wrap {
            border: 1px solid #e5e7eb;
            border-radius: 10px;
            overflow: hidden;
            background: #fff;
        }

        .detail-jo-table {
            margin-bottom: 0;
            min-width: 860px;
        }

        .detail-jo-table thead th {
            background: #f8fafc;
            color: #334155;
            font-size: 11px;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: .3px;
            white-space: nowrap;
        }

        .detail-jo-sortable {
            cursor: pointer;
            user-select: none;
            position: relative;
            padding-right: 18px !important;
        }

        .detail-jo-sortable:hover {
            background: #eef2f7;
        }

        .detail-jo-sortable::after {
            content: "\2195";
            position: absolute;
            right: 5px;
            top: 50%;
            transform: translateY(-50%);
            font-size: 10px;
            color: #94a3b8;
            opacity: 0.7;
        }

        .detail-jo-sortable.is-sorted-asc::after {
            content: "\25B2";
            opacity: 1;
            color: #2563eb;
        }

        .detail-jo-sortable.is-sorted-desc::after {
            content: "\25BC";
            opacity: 1;
            color: #2563eb;
        }

        .detail-jo-table tbody td {
            font-size: 12px;
            vertical-align: middle;
        }

        tr.detail-jo-row.detail-jo-over-sla td {
            background: #fee2e2 !important;
            color: #991b1b;
        }

        tr.detail-jo-row.detail-jo-over-sla td:nth-child(11) {
            font-weight: 700;
        }

        .detail-jo-table tfoot td {
            background: #f1f5f9;
            font-size: 12px;
            font-weight: 700;
            color: #1f2937;
            border-top: 2px solid #cbd5e1;
        }

        .detail-jo-footer {
            border-top: 1px solid #ecebe8;
            background: #fafaf8;
            padding: 10px 16px;
            display: flex;
            align-items: center;
            justify-content: flex-end;
            gap: 8px;
        }

        .detail-jo-export-btn {
            border: 1px solid #1d4ed8;
            border-radius: 9px;
            background: #2563eb;
            color: #fff;
            min-height: 36px;
            padding: 7px 14px;
            font-size: 12px;
            font-weight: 700;
            cursor: pointer;
        }

        .detail-jo-export-btn:hover {
            background: #1d4ed8;
        }

        .detail-jo-close-btn {
            border: 1px solid #d1d5db;
            border-radius: 9px;
            background: #fff;
            color: #374151;
            min-height: 36px;
            padding: 7px 14px;
            font-size: 12px;
            font-weight: 700;
            cursor: pointer;
        }

        .detail-jo-close-btn:hover {
            background: #f8fafc;
        }


        .assign-job-backdrop {
            position: fixed;
            inset: 0;
            z-index: 1100;
            display: none;
            align-items: center;
            justify-content: center;
            background: rgba(14, 17, 22, 0.45);
            backdrop-filter: blur(4px);
            padding: 14px;
        }

        .assign-job-backdrop.open {
            display: flex;
        }

        .assign-job-modal {
            width: 100%;
            max-width: 1120px;
            max-height: 90vh;
            overflow: hidden;
            display: flex;
            flex-direction: column;
            background: #fff;
            border-radius: 18px;
            border: 1px solid #e5e3df;
            box-shadow: 0 18px 42px rgba(0, 0, 0, .22);
        }

        .assign-job-modal-header {
            display: flex;
            align-items: flex-start;
            justify-content: space-between;
            gap: 10px;
            padding: 18px 20px 12px;
            border-bottom: 1px solid #ecebe8;
            background: #fafaf8;
        }

        .assign-job-title-wrap h4 {
            margin: 0;
            color: #1a1a18;
            font-size: 18px;
            font-weight: 700;
            line-height: 1.2;
        }

        .assign-job-title-wrap p {
            margin: 4px 0 0;
            color: #6b7280;
            font-size: 12px;
        }

        .assign-job-close {
            border: 1px solid #d6d3cf;
            border-radius: 9px;
            background: #fff;
            color: #5c5c55;
            width: 32px;
            height: 32px;
            font-size: 18px;
            line-height: 1;
            cursor: pointer;
        }

        .assign-job-body {
            padding: 16px 20px 20px;
            overflow: auto;
            flex: 1;
            min-height: 0;
        }

        .assign-info-grid {
            display: grid;
            grid-template-columns: repeat(3, minmax(0, 1fr));
            gap: 8px;
            margin-bottom: 14px;
        }

        .assign-info-item {
            border: 1px solid #e6ecf3;
            background: #f8fbff;
            border-radius: 10px;
            padding: 8px 10px;
        }

        .assign-info-label {
            display: block;
            font-size: 10px;
            font-weight: 700;
            letter-spacing: .5px;
            text-transform: uppercase;
            color: #8b98a8;
            margin-bottom: 2px;
        }

        .assign-info-value {
            display: block;
            font-size: 13px;
            font-weight: 600;
            color: #1a1a18;
            word-break: break-word;
        }

        .assign-section-label {
            margin: 0 0 8px;
            color: #6b7280;
            font-size: 11px;
            text-transform: uppercase;
            letter-spacing: .7px;
            font-weight: 700;
        }

        .assign-jo-type {
            display: flex;
            gap: 8px;
            margin-bottom: 10px;
        }

        .assign-jo-type-btn {
            flex: 1;
            border: 1px solid #d6d3cf;
            border-radius: 10px;
            background: #fff;
            color: #5c5c55;
            font-size: 12px;
            font-weight: 600;
            padding: 9px 10px;
            cursor: pointer;
            min-height: 38px;
        }

        .assign-jo-type-btn.active {
            border-color: #0a5c48;
            background: #e4f4ef;
            color: #0a5c48;
        }

        .assign-jo-type-btn:disabled {
            cursor: not-allowed;
            opacity: .55;
            background: #f3f4f6;
            border-color: #e5e7eb;
            color: #9ca3af;
        }

        .assign-field {
            margin-bottom: 10px;
        }

        .assign-field-label {
            display: block;
            margin: 0 0 6px;
            color: #5c5c55;
            font-size: 12px;
            font-weight: 700;
        }

        .assign-field-hint {
            display: block;
            margin-top: 6px;
            color: #6b7280;
            font-size: 11px;
            font-weight: 600;
        }

        .assign-field input,
        .assign-field textarea {
            width: 100%;
            border: 1px solid #d1cfc9;
            border-radius: 10px;
            font-size: 13px;
            padding: 8px 10px;
            min-height: 38px;
            background: #fff;
            color: #1a1a18;
            box-sizing: border-box;
        }

        .assign-field textarea.assign-dashboard-note-textarea {
            min-height: 88px;
            line-height: 1.45;
            resize: vertical;
        }

        .assign-field textarea.assign-dashboard-note-textarea:focus {
            outline: none;
            border-color: #0a5c48;
            box-shadow: 0 0 0 2px rgba(10, 92, 72, 0.15);
        }

        .assign-report-delete-card {
            border: 1px solid #fecaca;
            border-radius: 12px;
            background: #fffbfb;
            padding: 14px;
            margin-top: 12px;
        }

        .assign-report-delete-summary {
            margin: 0 0 10px;
            font-size: 13px;
            font-weight: 600;
            color: #991b1b;
            line-height: 1.45;
        }

        .assign-report-delete-submit-btn {
            border-color: #b91c1c;
            background: #b91c1c;
        }

        .assign-field select {
            width: 100%;
            border: 1px solid #d1cfc9;
            border-radius: 10px;
            font-size: 13px;
            font-weight: 600;
            padding: 8px 36px 8px 10px;
            min-height: 38px;
            background-color: #fff;
            color: #1a1a18;
            appearance: none;
            -webkit-appearance: none;
            -moz-appearance: none;
            transition: border-color .18s ease, box-shadow .18s ease, background-color .18s ease;
            background-image:
                linear-gradient(45deg, transparent 50%, #64748b 50%),
                linear-gradient(135deg, #64748b 50%, transparent 50%),
                linear-gradient(to right, #eef2f7, #eef2f7);
            background-position:
                calc(100% - 16px) calc(50% - 2px),
                calc(100% - 11px) calc(50% - 2px),
                calc(100% - 34px) 50%;
            background-size: 5px 5px, 5px 5px, 1px 22px;
            background-repeat: no-repeat;
            cursor: pointer;
        }

        .assign-field select:hover {
            border-color: #8da0b8;
            background-color: #f8fafc;
        }

        .assign-field select:focus {
            outline: none;
            border-color: #0a5c48;
            box-shadow: 0 0 0 3px rgba(10, 92, 72, 0.15);
            background-color: #fff;
        }

        .assign-field select:disabled {
            cursor: not-allowed;
            background-color: #f3f4f6;
            color: #9ca3af;
            border-color: #e5e7eb;
        }

        .assign-area-hint {
            display: block;
            margin-top: 6px;
            color: #64748b;
            font-size: 11px;
            font-weight: 600;
        }

        .assign-area-hint.active {
            color: #0a5c48;
        }

        .assign-area-hint.error {
            color: #b91c1c;
        }

        .assign-pick-row {
            border: 1px solid #d1d5db;
            border-radius: 10px;
            background: #f9fafb;
            padding: 10px;
        }

        .assign-pick-btn {
            width: 100%;
            border: 1px solid #0a5c48;
            border-radius: 9px;
            background: #e4f4ef;
            color: #0a5c48;
            font-size: 12px;
            font-weight: 700;
            padding: 8px 10px;
            min-height: 38px;
            cursor: pointer;
        }

        .assign-pick-btn:hover {
            filter: brightness(.97);
        }

        .assign-picked-jo {
            margin-top: 8px;
            color: #5c5c55;
            font-size: 12px;
            line-height: 1.35;
            min-height: 16px;
        }

        .assign-order-card {
            border: 1px solid #dbeafe;
            background: #eff6ff;
            border-radius: 10px;
            padding: 10px;
            display: grid;
            grid-template-columns: repeat(3, minmax(0, 1fr));
            gap: 8px;
            margin-bottom: 10px;
        }

        .assign-status-grid {
            display: grid;
            grid-template-columns: repeat(2, minmax(0, 1fr));
            gap: 8px;
            margin-bottom: 12px;
        }

        .assign-status-btn {
            border: 1px solid #d6d3cf;
            border-radius: 10px;
            background: #fff;
            font-size: 12px;
            font-weight: 600;
            color: #5c5c55;
            padding: 9px 10px;
            min-height: 38px;
            cursor: pointer;
        }

        .assign-status-btn.active {
            border-color: #0a5c48;
            background: #e4f4ef;
            color: #0a5c48;
        }

        .assign-status-btn:disabled {
            border-color: #d1d5db;
            background: #f3f4f6;
            color: #9ca3af;
            cursor: not-allowed;
            opacity: 1;
        }

        .assign-feedback {
            display: none;
            border-radius: 10px;
            border: 1px solid transparent;
            padding: 9px 11px;
            font-size: 12px;
            font-weight: 600;
            line-height: 1.4;
            margin: 6px 0 12px;
        }

        .assign-feedback.is-visible {
            display: block;
        }

        .assign-feedback.error {
            color: #991b1b;
            background: #fef2f2;
            border-color: #fecaca;
        }

        .assign-feedback.success {
            color: #166534;
            background: #ecfdf5;
            border-color: #bbf7d0;
        }

        .assign-actions {
            display: flex;
            justify-content: flex-end;
            gap: 8px;
        }

        .assign-footer-left-actions {
            display: flex;
            align-items: center;
            gap: 8px;
        }

        .assign-job-footer {
            border-top: 1px solid #ecebe8;
            padding: 12px 20px;
            background: #fafaf8;
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 10px;
        }

        .assign-action-btn {
            border: 1px solid #d1cfc9;
            border-radius: 10px;
            padding: 8px 14px;
            font-size: 12px;
            font-weight: 700;
            min-height: 38px;
            cursor: pointer;
        }

        .assign-action-btn.cancel {
            background: #fff;
            color: #5c5c55;
        }

        .assign-action-btn.submit {
            background: #0a5c48;
            color: #fff;
            border-color: #0a5c48;
        }

        .assign-action-btn.submit-secondary {
            background: #1d4ed8;
            color: #fff;
            border-color: #1d4ed8;
        }

        .assign-action-btn:disabled {
            opacity: .65;
            cursor: not-allowed;
        }

        .assign-action-btn.submit.is-loading {
            position: relative;
            padding-left: 34px;
        }

        .assign-action-btn.submit.is-loading:before {
            content: "";
            position: absolute;
            left: 12px;
            top: 50%;
            width: 14px;
            height: 14px;
            margin-top: -7px;
            border-radius: 50%;
            border: 2px solid rgba(255, 255, 255, 0.45);
            border-top-color: #fff;
            animation: assign-spin .8s linear infinite;
        }

        @keyframes assign-spin {
            to {
                transform: rotate(360deg);
            }
        }

        .assign-jo-info-backdrop {
            z-index: 1300;
            background: rgba(14, 17, 22, 0.62);
            backdrop-filter: blur(6px);
        }

        .assign-jo-info-backdrop.open {
            justify-content: flex-start;
            align-items: flex-start;
            padding: 10px;
        }

        .assign-jo-info-modal {
            width: min(calc(100vw - 20px), 1760px);
            max-width: none;
            max-height: calc(100vh - 20px);
            overflow: hidden;
            display: flex;
            flex-direction: column;
            margin: 0;
            background: #fff;
            border-radius: 14px;
            border: 1px solid #d9dde4;
            box-shadow: 0 20px 48px rgba(0, 0, 0, .25);
        }

        .assign-jo-info-modal .assign-job-body {
            flex: 1 1 auto;
            min-height: 0;
            overflow: auto;
            display: flex;
            flex-direction: column;
        }

        .assign-report-backdrop {
            z-index: 1250;
            background: rgba(14, 17, 22, 0.6);
            backdrop-filter: blur(6px);
        }

        .assign-report-modal {
            width: 100%;
            max-width: 1120px;
            max-height: 90vh;
            overflow: hidden;
            display: flex;
            flex-direction: column;
            background: #fff;
            border-radius: 18px;
            border: 1px solid #d9dde4;
            box-shadow: 0 20px 48px rgba(0, 0, 0, .25);
        }

        .assign-report-header {
            display: flex;
            align-items: flex-start;
            gap: 12px;
            padding: 18px 20px 14px;
            border-bottom: 1px solid #ecebe8;
            background: #fafaf8;
        }

        .assign-report-icon {
            width: 42px;
            height: 42px;
            border-radius: 12px;
            background: #dcfce7;
            color: #166534;
            font-size: 21px;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            flex-shrink: 0;
        }

        .assign-report-title-wrap {
            flex: 1;
            min-width: 0;
        }

        .assign-report-title-wrap h4 {
            margin: 0;
            color: #1a1a18;
            font-size: 18px;
            font-weight: 700;
            line-height: 1.2;
        }

        .assign-report-title-wrap p {
            margin: 4px 0 0;
            color: #6b7280;
            font-size: 12px;
            line-height: 1.35;
        }

        .assign-report-close {
            border: 1px solid #d6d3cf;
            border-radius: 9px;
            background: #fff;
            color: #5c5c55;
            width: 32px;
            height: 32px;
            font-size: 18px;
            line-height: 1;
            cursor: pointer;
            flex-shrink: 0;
        }

        .assign-report-body {
            flex: 1 1 auto;
            min-height: 0;
            overflow: auto;
            padding: 16px 20px 20px;
            scrollbar-gutter: stable;
        }

        .assign-report-feedback {
            display: none;
            border-radius: 10px;
            border: 1px solid transparent;
            padding: 9px 11px;
            font-size: 12px;
            font-weight: 600;
            line-height: 1.4;
            margin: 0 0 10px;
        }

        .assign-report-feedback.is-visible {
            display: block;
        }

        .assign-report-feedback.error {
            color: #991b1b;
            background: #fef2f2;
            border-color: #fecaca;
        }

        .assign-report-feedback.success {
            color: #166534;
            background: #ecfdf3;
            border-color: #bbf7d0;
        }

        .assign-report-summary {
            display: grid;
            grid-template-columns: repeat(4, minmax(0, 1fr));
            gap: 8px;
            margin-bottom: 12px;
            border: 1px solid #e4ebf2;
            border-radius: 12px;
            background: #f8fbff;
            padding: 8px;
        }

        .assign-report-table-wrap {
            border: 1px solid #e6ecf3;
            background: #f8fbff;
            border-radius: 12px;
            margin-bottom: 12px;
            overflow: auto;
            max-height: 320px;
        }

        .assign-report-table {
            width: 100%;
            min-width: 1120px;
            border-collapse: collapse;
        }

        .assign-report-table th {
            background: #eef4fb;
            color: #5b6b7a;
            font-size: 11px;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: .35px;
            padding: 9px 10px;
            border-bottom: 1px solid #dbe4ef;
            white-space: nowrap;
            text-align: left;
            position: sticky;
            top: 0;
            z-index: 2;
        }

        .assign-report-table td {
            font-size: 12px;
            color: #1f2937;
            padding: 10px;
            border-bottom: 1px solid #e8edf4;
            vertical-align: top;
            white-space: nowrap;
        }

        .assign-report-table td.col-customer {
            white-space: normal;
            min-width: 220px;
            line-height: 1.35;
        }

        .assign-report-table td.col-action {
            white-space: nowrap;
            min-width: 135px;
        }

        .assign-report-table tbody tr:last-child td {
            border-bottom: 0;
        }

        .assign-report-table tbody tr:hover td {
            background: #f7fafc;
        }

        .assign-report-status {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            padding: 3px 8px;
            border-radius: 14px;
            font-size: 11px;
            font-weight: 700;
        }

        .assign-report-status.done {
            background: #dcfce7;
            color: #166534;
        }

        .assign-report-status.pending {
            background: #ffedd5;
            color: #9a3412;
        }

        .assign-jo-filter-wrap {
            display: flex;
            gap: 8px;
            margin-bottom: 10px;
            align-items: center;
        }

        .assign-jo-filter-wrap select {
            min-width: 180px;
            max-width: 280px;
        }

        .assign-jo-text-col {
            white-space: nowrap;
            vertical-align: middle;
        }

        .assign-jo-text-plain {
            display: inline-block;
            max-width: 100%;
            white-space: nowrap;
        }

        .assign-jo-expandable {
            display: inline-flex;
            align-items: flex-start;
            gap: 8px;
            max-width: 100%;
            vertical-align: top;
        }

        .assign-jo-expandable .assign-jo-text-content {
            display: inline-block;
            max-width: 380px;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
            vertical-align: top;
        }

        .assign-jo-expandable.is-expanded .assign-jo-text-content {
            max-width: 560px;
            white-space: normal;
            word-break: break-word;
            overflow: visible;
            text-overflow: clip;
        }

        .assign-jo-text-toggle {
            flex-shrink: 0;
            border: none;
            background: transparent;
            color: #0a5c48;
            font-size: 11px;
            font-weight: 700;
            cursor: pointer;
            padding: 0;
            line-height: 1.35;
            text-decoration: underline;
            white-space: nowrap;
        }

        .assign-jo-text-toggle:hover {
            color: #064e3b;
        }

        .assign-jo-text-toggle[hidden] {
            display: none !important;
        }

        .assign-jo-expandable:not(.has-overflow) .assign-jo-text-content {
            max-width: none;
            overflow: visible;
            text-overflow: clip;
        }

        .assign-its-hide-unit {
            display: none !important;
        }

        .assign-report-edit-btn {
            border: 1px solid #fcd34d;
            border-radius: 8px;
            background: #fffbeb;
            color: #92400e;
            font-size: 11px;
            font-weight: 700;
            min-height: 30px;
            padding: 4px 10px;
            cursor: pointer;
        }

        .assign-report-delete-btn {
            border: 1px solid #fca5a5;
            border-radius: 8px;
            background: #fff1f2;
            color: #b91c1c;
            font-size: 11px;
            font-weight: 700;
            min-height: 30px;
            padding: 4px 10px;
            cursor: pointer;
        }

        .assign-report-delete-btn:disabled {
            opacity: .65;
            cursor: not-allowed;
        }

        .assign-report-training-btn,
        .assign-report-detail-btn {
            border: 1px solid #93c5fd;
            border-radius: 8px;
            background: #eff6ff;
            color: #1d4ed8;
            font-size: 11px;
            font-weight: 700;
            min-height: 30px;
            padding: 4px 10px;
            cursor: pointer;
        }

        .assign-report-install-btn {
            border: 1px solid #86efac;
            border-radius: 8px;
            background: #ecfdf5;
            color: #047857;
            font-size: 11px;
            font-weight: 700;
            min-height: 30px;
            padding: 4px 10px;
            cursor: pointer;
            display: inline-flex;
            align-items: center;
            text-decoration: none;
            line-height: 1.2;
        }

        .assign-report-install-btn:hover,
        .assign-report-install-btn:focus {
            background: #d1fae5;
            color: #065f46;
            text-decoration: none;
        }

        .assign-report-action-wrap {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            flex-wrap: nowrap;
            white-space: nowrap;
        }

        .assign-report-empty {
            padding: 24px;
            text-align: center;
            color: #64748b;
            font-size: 12px;
            font-weight: 600;
        }

        .assign-report-assign-card {
            border: 1px solid #dbe5ef;
            border-radius: 12px;
            background: #fbfdff;
            padding: 14px;
            margin-top: 12px;
        }

        .assign-report-assign-card .assign-section-label {
            margin-top: 10px;
        }

        .assign-report-assign-card .assign-section-label:first-child {
            margin-top: 0;
        }

        .assign-report-assign-intro {
            display: flex;
            flex-direction: column;
            gap: 8px;
        }

        .assign-report-assign-hint {
            margin: 0;
            font-size: 12px;
            color: #64748b;
            line-height: 1.45;
        }

        .assign-report-add-btn {
            align-self: flex-start;
            border: 1px dashed #0a5c48;
            border-radius: 10px;
            background: #fff;
            color: #0a5c48;
            font-size: 12px;
            font-weight: 700;
            min-height: 38px;
            padding: 8px 14px;
            cursor: pointer;
        }

        .assign-report-add-btn:hover {
            background: #f0fdf9;
        }

        .assign-report-inline-actions {
            display: flex;
            justify-content: flex-end;
            gap: 8px;
            margin-top: 10px;
        }

        .assign-report-submit-btn {
            border: 1px solid #0a5c48;
            border-radius: 10px;
            background: #0a5c48;
            color: #fff;
            font-size: 12px;
            font-weight: 700;
            min-height: 38px;
            padding: 8px 14px;
            cursor: pointer;
        }

        .assign-report-submit-btn:disabled {
            opacity: .65;
            cursor: not-allowed;
        }

        .assign-report-submit-btn.is-loading::after {
            content: " ...";
        }

        .assign-report-submit-btn--secondary {
            border-color: #1d4ed8;
            background: #1d4ed8;
        }

        .assign-report-actions {
            display: flex;
            justify-content: flex-end;
            padding: 10px 20px 14px;
            background: #fff;
            border-top: 1px solid #eef2f7;
            flex: 0 0 auto;
        }

        .assign-report-close-btn {
            border: 1px solid #d1cfc9;
            border-radius: 10px;
            padding: 8px 14px;
            font-size: 12px;
            font-weight: 700;
            min-height: 38px;
            cursor: pointer;
            background: #fff;
            color: #5c5c55;
        }

        .assign-tech-modal-backdrop {
            position: fixed;
            inset: 0;
            z-index: 1085;
            background: rgba(17, 24, 39, 0.35);
            backdrop-filter: blur(2px);
            display: none;
            align-items: center;
            justify-content: center;
            padding: 18px;
        }

        .assign-tech-modal-backdrop.open {
            display: flex;
        }

        .assign-tech-modal {
            width: 100%;
            max-width: 620px;
            max-height: 88vh;
            overflow: auto;
            background: #fff;
            border-radius: 18px;
            border: 1px solid #e5e3df;
            box-shadow: 0 12px 28px rgba(0, 0, 0, 0.16);
            padding: 20px 20px 16px;
        }

        .assign-tech-modal-head {
            display: flex;
            justify-content: space-between;
            align-items: flex-start;
            gap: 10px;
            margin-bottom: 12px;
            border-bottom: 1px solid #e5e3df;
            padding-bottom: 10px;
        }

        .assign-tech-modal-head h4 {
            margin: 0;
            font-size: 32px;
            font-weight: 700;
            color: #1a1a18;
        }

        .assign-tech-modal-head p {
            margin: 2px 0 0;
            color: #6b7280;
            font-size: 11px;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: .5px;
        }

        .assign-tech-modal-close {
            border: 1px solid #d1cfc9;
            background: #fff;
            color: #6b7280;
            border-radius: 10px;
            width: 32px;
            height: 32px;
            font-size: 18px;
            font-weight: 700;
            line-height: 1;
            cursor: pointer;
            flex-shrink: 0;
        }

        .assign-closed-modal .modal-dialog {
            width: 92vw;
            max-width: 880px;
        }

        .assign-closed-modal .modal-content {
            border-radius: 20px;
            border: 0;
            box-shadow: 0 8px 40px rgba(0, 0, 0, 0.14), 0 2px 8px rgba(0, 0, 0, 0.08);
            overflow: hidden;
            padding: 18px 18px 14px;
            background: #f8fafc;
            position: relative;
            display: flex;
            flex-direction: column;
            max-height: 90vh;
        }

        .assign-closed-modal-top {
            padding-right: 26px;
        }

        .assign-closed-eyebrow {
            font-size: 10px;
            font-weight: 700;
            letter-spacing: 1px;
            text-transform: uppercase;
            color: #8d8d84;
            margin-bottom: 4px;
        }

        .assign-closed-modal-top h4 {
            margin: 0;
            font-size: 30px;
            font-weight: 700;
            color: #1f2937;
            line-height: 1.1;
            text-transform: uppercase;
        }

        .assign-closed-divider {
            height: 1px;
            background: #e5e3df;
            margin: 8px 0 12px;
        }

        .assign-closed-modal-close {
            border: 0;
            background: transparent;
            color: #7c7c73;
            border-radius: 0;
            width: 22px;
            height: 22px;
            font-size: 24px;
            line-height: 20px;
            padding: 0;
            opacity: 1;
            text-shadow: none;
            position: absolute;
            right: 18px;
            top: 14px;
            transition: color .15s ease;
        }

        .assign-closed-modal-close:hover {
            color: #111827;
            background: transparent;
        }

        .assign-closed-modal-body {
            overflow: hidden;
            background: transparent;
            padding: 0;
            display: flex;
            flex-direction: column;
            flex: 1;
            min-height: 0;
        }

        .assign-closed-modal-body #assignTotalJobContent,
        .assign-closed-modal-body #assignDayTotalJoContent {
            flex: 1;
            min-height: 0;
            display: flex;
            flex-direction: column;
            overflow: hidden;
        }

        .assign-closed-modal-body #assignDayTotalJoContent .assign-closed-table-wrap,
        .assign-closed-modal-body #assignTotalJobContent .assign-closed-table-wrap {
            flex: 1;
            min-height: 0;
            max-height: min(52vh, 460px);
            overflow: auto;
            -webkit-overflow-scrolling: touch;
        }

        .assign-totaljob-summary-wrap {
            display: grid;
            grid-template-columns: repeat(2, minmax(0, 1fr));
            gap: 12px;
            margin-bottom: 14px;
        }

        .assign-totaljob-summary {
            position: relative;
            display: flex;
            flex-direction: column;
            justify-content: center;
            gap: 6px;
            min-height: 74px;
            font-size: 12px;
            color: #64748b;
            background: linear-gradient(180deg, #ffffff 0%, #f8fafc 100%);
            border: 1px solid #dbe4ef;
            border-radius: 14px;
            padding: 12px 14px;
            box-shadow: 0 3px 10px rgba(15, 23, 42, 0.06);
            overflow: hidden;
        }

        .assign-totaljob-summary::before {
            content: "";
            position: absolute;
            left: 0;
            top: 0;
            width: 4px;
            height: 100%;
            background: #2563eb;
        }

        .assign-totaljob-summary--unit::before {
            background: #0f766e;
        }

        .assign-totaljob-summary-label {
            font-size: 11px;
            font-weight: 700;
            letter-spacing: .2px;
            text-transform: uppercase;
            color: #475569;
        }

        .assign-totaljob-summary strong {
            color: #111827;
            font-size: 28px;
            font-weight: 800;
            line-height: 1;
        }

        @media (max-width: 768px) {
            .assign-totaljob-summary-wrap {
                grid-template-columns: 1fr;
            }
        }

        .assign-closed-table-wrap {
            border: 1px solid #e5e7eb;
            border-radius: 12px;
            background: #ffffff;
            overflow: auto;
            max-height: none;
            box-shadow: 0 2px 6px rgba(15, 23, 42, 0.05);
            flex: 1;
            min-height: 0;
        }

        .assign-closed-table {
            width: 100%;
            border-collapse: separate;
            border-spacing: 0;
            min-width: 1140px;
        }

        .assign-closed-table thead th {
            position: sticky;
            top: 0;
            z-index: 2;
            background: #f1f5f9;
            color: #334155;
            font-size: 12px;
            font-weight: 700;
            text-align: left;
            padding: 10px 12px;
            border-bottom: 1px solid #dbe4ef;
            white-space: nowrap;
        }

        .assign-closed-table tbody td {
            font-size: 13px;
            color: #1f2937;
            padding: 9px 12px;
            border-bottom: 1px solid #edf2f7;
            vertical-align: top;
            white-space: nowrap;
        }

        .assign-closed-table thead th:first-child,
        .assign-closed-table tbody td:first-child {
            min-width: 168px;
            max-width: 220px;
            white-space: normal;
            line-height: 1.45;
            word-break: break-word;
        }

        .assign-closed-table tbody tr:nth-child(even):not(.assign-closed-group-row) td {
            background: #fbfdff;
        }

        .assign-closed-group-row td {
            background: #f8fafc !important;
            padding: 8px 12px !important;
            border-top: 1px solid #dde7f2;
            border-bottom: 1px solid #dde7f2;
        }

        .assign-closed-group-wrap {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 10px;
        }

        .assign-closed-group-wrap strong {
            font-size: 13px;
            color: #0f172a;
            font-weight: 700;
        }

        .assign-closed-job-badge {
            background: #dcfce7;
            color: #166534;
            border-radius: 999px;
            font-size: 11px;
            font-weight: 700;
            padding: 3px 9px;
            white-space: nowrap;
        }

        .assign-closed-loading {
            min-height: 180px;
            display: flex;
            align-items: center;
            justify-content: center;
            color: #475569;
            gap: 10px;
            font-size: 13px;
            font-weight: 600;
        }

        .assign-closed-loading-spinner {
            width: 20px;
            height: 20px;
            border: 3px solid #dbeafe;
            border-top-color: #2563eb;
            border-radius: 50%;
            animation: assignClosedSpin .9s linear infinite;
        }

        @keyframes assignClosedSpin {
            to {
                transform: rotate(360deg);
            }
        }

        .assign-closed-empty {
            min-height: 180px;
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            text-align: center;
            color: #6b7280;
            gap: 6px;
        }

        .assign-closed-empty-icon {
            font-size: 30px;
            line-height: 1;
        }

        .assign-closed-empty p {
            max-width: 260px;
            margin: 0;
            line-height: 1.45;
            font-size: 13px;
            font-weight: 500;
        }

        .assign-closed-actions {
            margin-top: 12px;
            display: flex;
            justify-content: flex-end;
        }

        .assign-closed-close-btn {
            border: 1px solid #d1d5db;
            background: #ffffff;
            color: #4b5563;
            border-radius: 10px;
            min-width: 122px;
            height: 38px;
            font-size: 13px;
            font-weight: 700;
            padding: 0 16px;
            transition: all .15s ease;
        }

        .assign-closed-close-btn:hover {
            border-color: #9ca3af;
            color: #111827;
            background: #f8fafc;
        }

        .assign-stock-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(170px, 1fr));
            gap: 10px;
            margin-bottom: 12px;
        }

        .assign-stock-card {
            border-radius: 12px;
            color: #fff;
            padding: 12px 14px;
            display: flex;
            justify-content: space-between;
            align-items: center;
            border: 0;
            width: 100%;
            text-align: left;
            cursor: pointer;
            transition: transform .16s ease, box-shadow .16s ease;
        }

        .assign-stock-card:hover {
            transform: translateY(-1px);
            box-shadow: 0 8px 16px rgba(15, 23, 42, .18);
        }

        .assign-stock-card:focus {
            outline: 2px solid rgba(255, 255, 255, .7);
            outline-offset: 1px;
        }

        .assign-stock-card.tone-1 { background: linear-gradient(135deg, #0A5C48, #0F8065); }
        .assign-stock-card.tone-2 { background: linear-gradient(135deg, #1D4ED8, #3B82F6); }
        .assign-stock-card.tone-3 { background: linear-gradient(135deg, #0891B2, #06B6D4); }
        .assign-stock-card.tone-4 { background: linear-gradient(135deg, #B45309, #F59E0B); }

        .assign-stock-card-name {
            font-size: 12px;
            font-weight: 600;
        }

        .assign-stock-card-type {
            font-size: 10px;
            opacity: .8;
            margin-top: 2px;
        }

        .assign-stock-card-val {
            font-size: 32px;
            font-weight: 700;
            line-height: 1;
        }

        .assign-stock-section-title {
            margin: 0 0 8px;
            color: #6b7280;
            font-size: 12px;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: .35px;
        }

        .assign-stock-empty {
            border: 1px dashed #d1d5db;
            border-radius: 10px;
            padding: 12px;
            text-align: center;
            color: #6b7280;
            font-size: 12px;
            background: #f9fafb;
            margin-bottom: 12px;
        }

        .assign-stock-loading {
            border: 1px solid #e5e7eb;
            border-radius: 12px;
            padding: 18px 14px;
            text-align: center;
            color: #6b7280;
            font-size: 12px;
            margin-bottom: 12px;
            background: #f8fafc;
        }

        .assign-stock-loading:before {
            content: "";
            display: inline-block;
            width: 14px;
            height: 14px;
            border-radius: 50%;
            border: 2px solid #cbd5e1;
            border-top-color: #0a5c48;
            margin-right: 8px;
            vertical-align: -2px;
            animation: assign-spin .8s linear infinite;
        }

        .assign-stock-list {
            border: 1px solid #e5e7eb;
            border-radius: 10px;
            overflow: hidden;
            margin-bottom: 12px;
        }

        .assign-stock-list-row {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 9px 12px;
            border-bottom: 1px solid #e5e7eb;
            background: #f9fafb;
            font-size: 13px;
        }

        .assign-stock-list-row:last-child {
            border-bottom: 0;
        }

        .assign-stock-badge {
            background: #d1fae5;
            color: #065f46;
            border-radius: 999px;
            font-size: 12px;
            font-weight: 700;
            padding: 3px 10px;
        }

        .assign-stock-total {
            background: #e4f4ef;
            border-radius: 10px;
            padding: 10px 12px;
            display: flex;
            justify-content: space-between;
            align-items: center;
            color: #0a5c48;
            font-weight: 700;
            margin-bottom: 12px;
        }

        .assign-tech-modal-footer {
            border-top: 1px solid #e5e3df;
            padding-top: 12px;
            text-align: right;
        }

        .assign-tech-modal-btn {
            border: 1px solid #d1cfc9;
            background: #fff;
            color: #6b7280;
            border-radius: 10px;
            min-width: 100px;
            height: 36px;
            font-size: 12px;
            font-weight: 700;
            cursor: pointer;
        }

        .assign-tech-detail-backdrop {
            position: fixed;
            inset: 0;
            z-index: 1090;
            background: rgba(17, 24, 39, 0.45);
            backdrop-filter: blur(2px);
            display: none;
            align-items: center;
            justify-content: center;
            padding: 18px;
        }

        .assign-tech-detail-backdrop.open {
            display: flex;
        }

        .assign-tech-detail-modal {
            width: 100%;
            max-width: 840px;
            max-height: 88vh;
            display: flex;
            flex-direction: column;
            background: #fff;
            border-radius: 16px;
            border: 1px solid #dbe4ef;
            box-shadow: 0 18px 32px rgba(0, 0, 0, 0.22);
            overflow: hidden;
        }

        .assign-tech-detail-head {
            display: flex;
            justify-content: space-between;
            align-items: flex-start;
            gap: 10px;
            padding: 14px 16px 10px;
            border-bottom: 1px solid #e5e7eb;
            background: #f8fafc;
        }

        .assign-tech-detail-head h4 {
            margin: 0;
            font-size: 18px;
            font-weight: 700;
            color: #111827;
        }

        .assign-tech-detail-head p {
            margin: 2px 0 0;
            font-size: 12px;
            color: #6b7280;
        }

        .assign-tech-detail-close {
            border: 1px solid #d1d5db;
            background: #fff;
            color: #6b7280;
            border-radius: 10px;
            width: 32px;
            height: 32px;
            font-size: 18px;
            line-height: 1;
            cursor: pointer;
            flex-shrink: 0;
        }

        .assign-tech-detail-body {
            padding: 12px 16px;
            overflow: auto;
            min-height: 0;
        }

        .assign-tech-detail-search {
            margin-bottom: 10px;
        }

        .assign-tech-detail-search input {
            width: 100%;
            border: 1px solid #d1d5db;
            border-radius: 10px;
            height: 36px;
            padding: 0 12px;
            font-size: 13px;
            color: #111827;
        }

        .assign-tech-detail-table-wrap {
            border: 1px solid #e5e7eb;
            border-radius: 10px;
            overflow: auto;
            max-height: 48vh;
            background: #fff;
        }

        .assign-tech-detail-table {
            width: 100%;
            min-width: 640px;
            border-collapse: separate;
            border-spacing: 0;
        }

        .assign-tech-detail-table thead th {
            position: sticky;
            top: 0;
            z-index: 1;
            background: #f1f5f9;
            color: #475569;
            font-size: 12px;
            font-weight: 700;
            padding: 10px 12px;
            border-bottom: 1px solid #dbe4ef;
            text-align: left;
            white-space: nowrap;
        }

        .assign-tech-detail-table tbody td {
            font-size: 12px;
            color: #1f2937;
            padding: 9px 12px;
            border-bottom: 1px solid #edf2f7;
        }

        .assign-tech-detail-table tbody tr:hover td {
            background: #f8fafc;
        }

        .assign-tech-detail-status {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            min-height: 22px;
            border-radius: 999px;
            padding: 2px 10px;
            font-size: 11px;
            font-weight: 700;
            white-space: nowrap;
            background: #eef2ff;
            color: #4338ca;
        }

        .assign-tech-detail-status.ready {
            background: #dcfce7;
            color: #166534;
        }

        .assign-tech-detail-footer {
            display: flex;
            align-items: flex-start;
            justify-content: space-between;
            gap: 12px;
            border-top: 1px solid #e5e7eb;
            padding: 10px 16px 12px;
            background: #fff;
        }

        .assign-tech-detail-legend {
            flex: 1;
            min-width: 0;
            font-size: 11px;
            line-height: 1.5;
            color: #64748b;
            text-align: left;
        }

        .assign-tech-detail-legend-item {
            display: flex;
            align-items: flex-start;
            gap: 6px;
            margin-bottom: 2px;
        }

        .assign-tech-detail-legend-item:last-child {
            margin-bottom: 0;
        }

        .assign-tech-detail-legend-item .assign-tech-detail-status {
            flex-shrink: 0;
            min-width: 42px;
        }

        .assign-tech-detail-footer .assign-tech-modal-btn {
            flex-shrink: 0;
            align-self: center;
        }

        .assign-tech-detail-feedback {
            display: none;
            border-radius: 8px;
            border: 1px solid transparent;
            padding: 8px 10px;
            margin-bottom: 10px;
            font-size: 12px;
            font-weight: 600;
        }

        .assign-tech-detail-feedback.is-visible {
            display: block;
        }

        .assign-tech-detail-feedback.error {
            color: #991b1b;
            background: #fef2f2;
            border-color: #fecaca;
        }

        .assign-report-remark-backdrop {
            z-index: 1350;
        }

        .assign-report-remark-backdrop.open {
            display: flex;
        }

        .assign-report-remark-modal {
            width: 100%;
            max-width: 560px;
            background: #ffffff;
            border-radius: 14px;
            border: 1px solid #dbe4ef;
            box-shadow: 0 18px 32px rgba(0, 0, 0, 0.22);
            overflow: hidden;
            display: flex;
            flex-direction: column;
            max-height: 82vh;
        }

        .assign-report-remark-head {
            display: flex;
            align-items: flex-start;
            justify-content: space-between;
            gap: 10px;
            padding: 14px 16px 10px;
            border-bottom: 1px solid #e5e7eb;
            background: #f8fafc;
        }

        .assign-report-remark-head h4 {
            margin: 0;
            font-size: 17px;
            font-weight: 700;
            color: #111827;
        }

        .assign-report-remark-head p {
            margin: 3px 0 0;
            font-size: 12px;
            color: #64748b;
        }

        .assign-report-remark-close {
            border: 1px solid #d4d8df;
            border-radius: 8px;
            background: #fff;
            color: #4b5563;
            width: 32px;
            height: 32px;
            line-height: 1;
            font-size: 18px;
            cursor: pointer;
        }

        .assign-report-remark-body {
            padding: 16px;
            overflow: auto;
        }

        .assign-report-remark-content {
            margin: 0;
            font-size: 13px;
            line-height: 1.5;
            color: #1f2937;
            white-space: pre-wrap;
            word-break: break-word;
            min-height: 72px;
        }

        .assign-report-remark-footer {
            padding: 12px 16px 14px;
            border-top: 1px solid #e5e7eb;
            text-align: right;
            background: #fafafa;
        }

        .assign-jo-search-wrap {
            display: flex;
            gap: 8px;
            margin-bottom: 12px;
        }

        .assign-jo-search-wrap input {
            flex: 1;
            min-width: 0;
            border: 1px solid #d1d5db;
            border-radius: 8px;
            padding: 8px 10px;
            font-size: 13px;
            min-height: 36px;
        }

        .assign-jo-search-wrap button {
            border: 1px solid #0a5c48;
            border-radius: 8px;
            min-width: 40px;
            background: #0a5c48;
            color: #fff;
            cursor: pointer;
        }

        .assign-jo-table-wrap {
            border: 1px solid #e5e7eb;
            border-radius: 8px;
            overflow: auto;
            flex: 1 1 auto;
            min-height: 320px;
            max-height: calc(100vh - 250px);
            background: #fff;
        }

        .assign-jo-table {
            width: 100%;
            border-collapse: collapse;
            min-width: 1540px;
        }

        .assign-jo-table th {
            background: #f3f4f6;
            color: #1f4b99;
            font-size: 12px;
            font-weight: 700;
            text-align: left;
            padding: 9px 10px;
            border-bottom: 1px solid #e5e7eb;
            white-space: nowrap;
        }

        .assign-jo-table td {
            font-size: 12px;
            color: #1f2937;
            padding: 9px 10px;
            border-bottom: 1px solid #eef2f7;
            vertical-align: middle;
            white-space: nowrap;
        }

        .assign-jo-table tbody tr:hover {
            background: #f8fafc;
        }

        .assign-jo-pick-btn {
            border: 1px solid #0a5c48;
            border-radius: 6px;
            background: #e4f4ef;
            color: #0a5c48;
            font-size: 11px;
            font-weight: 700;
            padding: 5px 9px;
            cursor: pointer;
        }

        .assign-jo-transfer-btn {
            border-color: #d97706;
            background: #fffbeb;
            color: #92400e;
        }

        .assign-jo-transfer-hint {
            margin-top: 4px;
            font-size: 10px;
            font-weight: 600;
            color: #92400e;
            line-height: 1.3;
        }

        .assign-jo-search-hint {
            margin-top: 6px;
            font-size: 11px;
            color: #6b7280;
        }

        .assign-jo-empty {
            text-align: center;
            color: #6b7280;
            padding: 14px 10px;
        }

        .assign-jo-loading {
            text-align: center;
            color: #0a5c48;
            padding: 16px 10px;
            font-weight: 600;
        }

        .assign-jo-pagination {
            margin-top: 10px;
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 8px;
        }

        .assign-jo-pagination button {
            border: 1px solid #d1d5db;
            border-radius: 6px;
            background: #fff;
            color: #4b5563;
            min-height: 34px;
            padding: 5px 11px;
            font-size: 12px;
            font-weight: 600;
            cursor: pointer;
        }

        .assign-jo-pagination button:disabled {
            opacity: .45;
            cursor: not-allowed;
        }

        #assignJoPageNumbers {
            display: flex;
            gap: 6px;
            align-items: center;
            justify-content: center;
            flex-wrap: wrap;
            flex: 1;
        }

        .assign-jo-page-btn.active {
            background: #0a5c48;
            border-color: #0a5c48;
            color: #fff;
        }

        .assign-jo-page-ellipsis {
            min-height: 34px;
            padding: 5px 6px;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            color: #9ca3af;
            font-weight: 700;
        }

        .assign-toast {
            position: fixed;
            right: 18px;
            bottom: 18px;
            z-index: 1400;
            min-width: 240px;
            max-width: min(420px, calc(100vw - 32px));
            border-radius: 10px;
            padding: 10px 12px;
            box-shadow: 0 10px 24px rgba(0, 0, 0, .2);
            color: #fff;
            font-size: 12px;
            line-height: 1.4;
            transform: translateY(14px);
            opacity: 0;
            pointer-events: none;
            transition: all .2s ease;
        }

        .assign-toast.show {
            opacity: 1;
            transform: translateY(0);
        }

        .assign-toast.success {
            background: #0a5c48;
        }

        .assign-toast.error {
            background: #b91c1c;
        }

        @media (min-width: 768px) and (max-width: 1199px) {
            .assign-topbar {
                padding: 10px 12px;
                min-height: auto;
                flex-wrap: wrap;
            }

            .assign-topbar-divider {
                display: none;
            }

            .assign-topbar-right {
                width: 100%;
                justify-content: flex-start;
                margin-left: 0;
            }

            .summary-grid {
                grid-template-columns: repeat(2, minmax(0, 1fr));
            }

            .assign-summary-value {
                font-size: 28px;
            }

            .section-header {
                padding: 10px 12px;
            }

            .performance-wrap {
                padding: 12px;
            }

            .perf-grid {
                grid-template-columns: repeat(2, minmax(0, 1fr));
            }

            .assign-table thead th {
                font-size: 10px;
            }

            .assign-table tbody td {
                font-size: 11px;
            }
        }

        @media (max-width: 767px) {
            .assign-dashboard {
                padding-bottom: 12px;
            }

            .content-header h1 {
                font-size: 20px;
                line-height: 1.25;
            }

            .assign-topbar {
                position: static;
                padding: 10px;
                border-radius: 10px;
                margin-bottom: 12px;
                min-height: auto;
                gap: 8px;
                flex-wrap: wrap;
            }

            .assign-brand-name {
                font-size: 13px;
            }

            .assign-brand-sub {
                font-size: 10px;
            }

            .assign-topbar-divider {
                display: none;
            }

            .assign-topbar-right {
                width: 100%;
                gap: 6px;
                margin-left: 0;
            }

            .assign-tabs {
                width: 100%;
                justify-content: flex-start;
                overflow-x: auto;
                white-space: nowrap;
                flex-wrap: nowrap;
                -webkit-overflow-scrolling: touch;
            }

            .assign-tab-btn {
                flex: 0 0 auto;
                min-width: 104px;
                font-size: 12px;
                padding: 7px 12px;
            }

            .assign-period {
                flex: 1 1 100%;
                width: 100%;
                min-width: 0;
                height: 36px;
                font-size: 13px;
            }

            .assign-btn {
                flex: 1 1 calc(50% - 3px);
                min-width: 0;
                min-height: 36px;
                font-size: 12px;
            }

            .summary-grid {
                grid-template-columns: 1fr;
                gap: 10px;
                margin-bottom: 12px;
            }

            .summary-card {
                padding: 12px 14px;
                border-radius: 12px;
            }

            .assign-summary-value {
                font-size: 24px;
            }

            .assign-summary-title {
                font-size: 10px;
            }

            .card-unit-row {
                margin-top: 8px;
                padding-top: 8px;
            }

            .section-box {
                margin-bottom: 12px;
                border-radius: 12px;
            }

            .section-header {
                padding: 10px 12px;
                align-items: flex-start;
                gap: 6px;
            }

            .section-title {
                font-size: 12px;
            }

            .panel-badge {
                font-size: 10px;
            }

            .area-filter-bar {
                width: 100%;
                margin-left: 0;
                padding-bottom: 2px;
                flex-wrap: nowrap;
                overflow-x: auto;
                -webkit-overflow-scrolling: touch;
                white-space: nowrap;
            }

            .area-filter-wrap {
                width: 100%;
                margin-left: 0;
                align-items: stretch;
                gap: 4px;
            }

            .area-group-filter-bar {
                width: 100%;
                flex-wrap: nowrap;
                overflow-x: auto;
                -webkit-overflow-scrolling: touch;
                white-space: nowrap;
            }

            .area-filter-btn {
                flex: 0 0 auto;
                font-size: 11px;
                padding: 6px 12px;
            }

            .performance-wrap {
                padding: 10px 12px;
                overflow-x: auto;
                -webkit-overflow-scrolling: touch;
            }

            .perf-grid {
                display: grid;
                grid-auto-flow: column;
                grid-auto-columns: minmax(200px, 1fr);
                grid-template-columns: none;
                gap: 8px;
                width: 100%;
                min-width: max-content;
            }

            .perf-item {
                padding: 9px 10px;
                min-height: 56px;
            }

            .perf-rank {
                width: 24px;
                font-size: 13px;
            }

            .perf-name {
                font-size: 11px;
            }

            .perf-area {
                font-size: 9px;
            }

            .perf-score {
                font-size: 16px;
            }

            .assign-table-wrap {
                max-height: none;
                overflow-y: visible;
            }

            .assign-table {
                width: max-content;
                min-width: 100%;
            }

            .assign-table thead th {
                top: 0;
                font-size: 10px;
                padding: 5px 4px;
            }

            .assign-day-no {
                font-size: 10px;
            }

            .assign-day-name {
                font-size: 8px;
            }

            .assign-table tbody td {
                font-size: 11px;
                height: 38px;
            }

            .col-no {
                width: 34px;
                min-width: 34px;
            }

            .col-name {
                width: 126px;
                min-width: 126px;
            }

            .col-total-assign {
                width: 66px;
                min-width: 66px;
            }

            .col-total {
                width: 66px;
                min-width: 66px;
            }

            .col-total-maint {
                width: 66px;
                min-width: 66px;
            }

            .col-stock {
                width: 92px;
                min-width: 92px;
            }


            .sticky-name {
                left: 34px;
            }

            .assign-table th.col-name,
            .assign-table td.col-name {
                left: 34px !important;
            }

            .assign-tech-modal {
                max-height: 92vh;
                padding: 14px 14px 12px;
            }

            .assign-stock-grid {
                grid-template-columns: repeat(2, minmax(0, 1fr));
            }

            .assign-closed-modal .modal-content {
                padding: 16px 14px 14px;
            }

            .assign-closed-modal-top h4 {
                font-size: 24px;
            }

            .assign-closed-modal-body {
                min-height: 0;
            }

            .assign-closed-table-wrap {
                min-height: 0;
            }

            .cell-name {
                padding: 0 8px;
                font-size: 11px;
            }

            .legend {
                gap: 8px;
                padding: 10px 12px;
            }

            .legend-item {
                font-size: 10px;
            }

            .legend-dot {
                width: 22px;
                height: 16px;
                font-size: 9px;
            }

            .assign-job-modal {
                max-width: 100%;
            }

            .assign-jo-info-modal {
                max-width: 100%;
            }

            .assign-report-modal {
                max-width: 100%;
            }

            .assign-job-modal-header {
                padding: 14px 14px 10px;
            }

            .assign-job-title-wrap h4 {
                font-size: 16px;
            }

            .assign-job-body {
                padding: 12px 14px 14px;
            }

            .detail-jo-dialog {
                width: calc(100vw - 20px);
                max-width: 100%;
                margin: 10px auto;
            }

            .detail-jo-body {
                padding: 10px 10px 8px;
            }


            .assign-info-grid,
            .assign-order-card {
                grid-template-columns: 1fr;
            }

            .assign-jo-type,
            .assign-status-grid {
                grid-template-columns: 1fr;
                display: grid;
            }

            .assign-actions {
                flex-direction: column-reverse;
            }

            .assign-footer-left-actions {
                width: 100%;
                flex-direction: column;
            }

            .assign-footer-left-actions .assign-action-btn,
            .assign-footer-left-actions .assign-report-close-btn {
                width: 100%;
            }

            .assign-report-header {
                padding: 14px 14px 10px;
            }

            .assign-report-body {
                padding: 12px 14px 16px;
            }

            .assign-report-actions {
                padding: 10px 14px 12px;
            }

            .assign-report-summary {
                grid-template-columns: 1fr;
            }

            .assign-action-btn {
                width: 100%;
            }

            .assign-jo-search-wrap {
                margin-bottom: 10px;
            }

            .assign-jo-pagination {
                flex-direction: column;
                align-items: stretch;
            }

            #assignJoPageNumbers {
                justify-content: flex-start;
            }
        }

        @media (max-width: 640px) {
            .assign-stock-grid {
                grid-template-columns: 1fr !important;
            }
        }

        .perf-chart-section .section-header {
            flex-wrap: wrap;
            gap: 10px;
        }

        .perf-chart-controls {
            margin-left: auto;
            display: flex;
            align-items: center;
            gap: 8px;
            flex-wrap: wrap;
        }

        .perf-chart-select {
            height: 30px;
            border-radius: 7px;
            border: 1px solid #d1cfc9;
            font-size: 12px;
            padding: 4px 8px;
            min-width: 190px;
            max-width: 100%;
            background: #fff;
            color: #1a1a18;
        }

        .perf-chart-note-wrap {
            padding: 0 16px 10px;
        }

        .perf-chart-note {
            font-size: 11px;
            color: #9b9b93;
            margin: 0;
        }

        .perf-chart-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 14px;
            padding: 0 16px 16px;
        }

        .perf-chart-panel {
            border: 1px solid #e5e3df;
            border-radius: 12px;
            background: #fcfcfb;
            padding: 12px 12px 8px;
            min-width: 0;
        }

        .perf-chart-panel-title {
            margin: 0 0 4px;
            font-size: 12px;
            font-weight: 700;
            color: #1a1a18;
        }

        .perf-chart-panel-sub {
            margin: 0 0 10px;
            font-size: 11px;
            color: #9b9b93;
        }

        .perf-chart-panel-top {
            margin-bottom: 10px;
        }

        .perf-chart-panel-heading {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 8px;
        }

        .perf-chart-panel-heading .perf-chart-panel-title {
            margin: 0;
        }

        .perf-daily-week-nav {
            display: flex;
            align-items: center;
            gap: 4px;
            flex-shrink: 0;
        }

        .perf-daily-week-btn {
            width: 28px;
            height: 28px;
            border: 1px solid #d1cfc9;
            border-radius: 7px;
            background: #fff;
            color: #1a1a18;
            font-size: 16px;
            line-height: 1;
            padding: 0;
            cursor: pointer;
        }

        .perf-daily-week-btn:hover:not(:disabled) {
            border-color: #0a5c48;
            color: #0a5c48;
        }

        .perf-daily-week-btn:disabled {
            opacity: 0.4;
            cursor: not-allowed;
        }

        .perf-chart-canvas {
            width: 100%;
            height: 280px;
            min-height: 220px;
        }

        @media (max-width: 992px) {
            .perf-chart-grid {
                grid-template-columns: 1fr;
            }

            .perf-chart-controls {
                margin-left: 0;
                width: 100%;
            }

            .perf-chart-select {
                width: 100%;
            }
        }
    </style>

    <section class="content-header">
        <h1>Dashboard <small>Assignment Job IT Support</small></h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Dashboard</a></li>
            <li class="active">Assignment Job IT Support</li>
        </ol>
    </section>

    <section class="content assign-dashboard">
        <div class="assign-topbar">
            <div class="assign-brand">
                <div class="assign-brand-icon" aria-hidden="true">EG</div>
                <div>
                    <div class="assign-brand-name">EasyGo</div>
                    <div class="assign-brand-sub">Monitoring Aktivitas</div>
                </div>
            </div>
            <div class="assign-topbar-divider" aria-hidden="true"></div>

            <div class="assign-tabs">
                <button type="button" class="assign-tab-btn" data-tab="teknisi" onclick="window.location.href='dashboard_assign_job.aspx';">Teknisi</button>
                <button type="button" class="assign-tab-btn active" data-tab="itsupport">IT Support</button>
            </div>

            <div class="assign-topbar-right">
                <button type="button" class="btn btn-primary assign-btn assign-create-jo-btn" id="assignCreateJoBtn">Buat JO Training/Visit</button>
                <asp:TextBox ID="txtPeriode" runat="server" CssClass="form-control assign-period js-periode-picker" placeholder="yyyy-MM" autocomplete="off"></asp:TextBox>
                <asp:Button ID="btnApply" runat="server" CssClass="btn btn-primary assign-btn" Text="Apply" OnClick="btnApply_Click" OnClientClick="showOverlay();" />
                <asp:Button ID="btnReset" runat="server" CssClass="btn btn-default assign-btn" Text="Reset" OnClick="btnReset_Click" />
            </div>
        </div>

        <asp:HiddenField ID="hfActiveTab" runat="server" Value="itsupport" />
        <asp:HiddenField ID="hfDetailStatus" runat="server" Value="all" />
        <asp:HiddenField ID="hfIsTechnician" runat="server" Value="0" />
        <asp:HiddenField ID="hfDetailJobType" runat="server" Value="all" />
        <asp:HiddenField ID="hfDetailMetric" runat="server" Value="jo" />
        <asp:LinkButton ID="btnTabRefresh" runat="server" CssClass="assign-hidden-trigger" CausesValidation="false" OnClick="btnTabRefresh_Click">refresh</asp:LinkButton>
        <asp:LinkButton ID="btnOpenDetail" runat="server" CssClass="assign-hidden-trigger" CausesValidation="false" OnClick="btnOpenDetail_Click">detail</asp:LinkButton>

        <div class="summary-grid">
            <div class="summary-card card-total">
                    <p class="assign-summary-title">Total Job Order</p>
                    <div class="summary-column-grid">
                        <div class="summary-column">
                            <p class="summary-column-title">Training</p>
                            <button type="button" class="summary-link-value" onclick="openSummaryDetailSplit('all','new_installation','jo');"><span id="lblTotalJONew" runat="server">0</span></button>
                            <div class="summary-unit-line">
                                <span class="summary-unit-label">Total Unit</span>
                                <span id="lblTotalUnitNew" runat="server" class="summary-unit-value">0</span>
                            </div>
                        </div>
                        <div class="summary-column">
                            <p class="summary-column-title">Visit</p>
                            <button type="button" class="summary-link-value" onclick="openSummaryDetailSplit('all','maintenance','jo');"><span id="lblTotalJOMaint" runat="server">0</span></button>
                            <div class="summary-unit-line">
                                <span class="summary-unit-label">Total Unit</span>
                                <span id="lblTotalUnitMaint" runat="server" class="summary-unit-value">0</span>
                            </div>
                        </div>
                    </div>
            </div>
            <div class="summary-card card-open">
                    <p class="assign-summary-title">JO Open</p>
                    <div class="summary-column-grid">
                        <div class="summary-column">
                            <p class="summary-column-title">Training</p>
                            <button type="button" class="summary-link-value" onclick="openSummaryDetailSplit('open','new_installation','jo');"><span id="lblTotalJOOpenNew" runat="server">0</span></button>
                            <div class="summary-unit-line">
                                <span class="summary-unit-label">Total Unit</span>
                                <span id="lblTotalUnitOpenNew" runat="server" class="summary-unit-value">0</span>
                            </div>
                        </div>
                        <div class="summary-column">
                            <p class="summary-column-title">Visit</p>
                            <button type="button" class="summary-link-value" onclick="openSummaryDetailSplit('open','maintenance','jo');"><span id="lblTotalJOOpenMaint" runat="server">0</span></button>
                            <div class="summary-unit-line">
                                <span class="summary-unit-label">Total Unit</span>
                                <span id="lblTotalUnitOpenMaint" runat="server" class="summary-unit-value">0</span>
                            </div>
                        </div>
                    </div>
            </div>
            <div class="summary-card card-scheduled">
                    <p class="assign-summary-title">JO Scheduled</p>
                    <div class="summary-column-grid">
                        <div class="summary-column">
                            <p class="summary-column-title">Training</p>
                            <button type="button" class="summary-link-value" onclick="openSummaryDetailSplit('scheduled','new_installation','jo');"><span id="lblTotalJOScheduledNew" runat="server">0</span></button>
                            <div class="summary-unit-line">
                                <span class="summary-unit-label">Total Unit</span>
                                <span id="lblTotalUnitScheduledNew" runat="server" class="summary-unit-value">0</span>
                            </div>
                        </div>
                        <div class="summary-column">
                            <p class="summary-column-title">Visit</p>
                            <button type="button" class="summary-link-value" onclick="openSummaryDetailSplit('scheduled','maintenance','jo');"><span id="lblTotalJOScheduledMaint" runat="server">0</span></button>
                            <div class="summary-unit-line">
                                <span class="summary-unit-label">Total Unit</span>
                                <span id="lblTotalUnitScheduledMaint" runat="server" class="summary-unit-value">0</span>
                            </div>
                        </div>
                    </div>
            </div>
            <div class="summary-card card-close">
                    <p class="assign-summary-title">JO Close</p>
                    <div class="summary-column-grid">
                        <div class="summary-column">
                            <p class="summary-column-title">Training</p>
                            <button type="button" class="summary-link-value" onclick="openSummaryDetailSplit('close','new_installation','jo');"><span id="lblTotalJOCloseNew" runat="server">0</span></button>
                            <div class="summary-unit-line">
                                <span class="summary-unit-label">Total Unit</span>
                                <span id="lblTotalUnitCloseNew" runat="server" class="summary-unit-value">0</span>
                            </div>
                        </div>
                        <div class="summary-column">
                            <p class="summary-column-title">Visit</p>
                            <button type="button" class="summary-link-value" onclick="openSummaryDetailSplit('close','maintenance','jo');"><span id="lblTotalJOCloseMaint" runat="server">0</span></button>
                            <div class="summary-unit-line">
                                <span class="summary-unit-label">Total Unit</span>
                                <span id="lblTotalUnitCloseMaint" runat="server" class="summary-unit-value">0</span>
                            </div>
                        </div>
                    </div>
            </div>
        </div>

        <div class="section-box" runat="server" visible="false">
            <div class="section-header">
                <h4 class="section-title">Data Performance Bulan Ini - <span id="lblActiveTabTitle" runat="server">IT Support</span></h4>
                <span class="panel-badge">Ranking</span>
            </div>
            <div class="performance-wrap">
                <div class="perf-grid" id="performanceWrap">
                    <asp:Literal ID="litPerformanceRows" runat="server"></asp:Literal>
                </div>
                <div id="lblPerformanceEmpty" runat="server" class="empty-text" visible="false">Belum ada data performance pada periode ini.</div>
            </div>
        </div>

        <div class="section-box">
            <div class="section-header">
                <h4 class="section-title">Jadwal Harian <span id="lblScheduleTabTitle" runat="server">IT Support</span></h4>
                <span id="lblMemberCount" runat="server" class="panel-badge">0 IT Support</span>
                <div class="area-filter-wrap">
                    <div class="area-group-filter-bar">
                        <asp:Repeater ID="rptAreaGroupTabs" runat="server" OnItemCommand="rptAreaGroupTabs_ItemCommand">
                            <ItemTemplate>
                                <asp:LinkButton
                                    ID="lnkAreaGroupTab"
                                    runat="server"
                                    Text='<%# Eval("AreaGroupName") %>'
                                    CommandName="SelectAreaGroupTab"
                                    CommandArgument='<%# Eval("AreaGroupID") %>'
                                    CssClass='<%# GetAreaGroupTabCss(Eval("AreaGroupID")) %>'
                                    CausesValidation="false"
                                    OnClientClick="showOverlay();">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <div class="area-filter-bar" style="display:none;" aria-hidden="true">
                        <asp:Repeater ID="rptRegionalTabs" runat="server" OnItemCommand="rptRegionalTabs_ItemCommand" Visible="false">
                            <ItemTemplate>
                                <asp:LinkButton
                                    ID="lnkRegionalTab"
                                    runat="server"
                                    Text='<%# Eval("SupAreaName") %>'
                                    CommandName="SelectRegionalTab"
                                    CommandArgument='<%# Eval("SupAreaID") %>'
                                    CssClass='<%# GetRegionalTabCss(Eval("SupAreaID")) %>'
                                    CausesValidation="false"
                                    OnClientClick="showOverlay();">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
            <div class="schedule-wrap">
                <div class="assign-table-wrap">
                        <table class="assign-table">
                            <thead>
                                <tr>
                                    <th class="col-no sticky-no">No</th>
                                    <th class="col-name sticky-name">Nama</th>
                                    <th class="col-total-assign">Total JO<br />Assign</th>
                                    <th class="col-total">Total Closed JO Training</th>
                                    <th class="col-total-maint">Total Closed JO Visit</th>
                                    <asp:Literal ID="litScheduleHeader" runat="server"></asp:Literal>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Literal ID="litScheduleRows" runat="server"></asp:Literal>
                            </tbody>
                            <tbody>
                                <asp:Literal ID="litCapacityRows" runat="server"></asp:Literal>
                            </tbody>
                        </table>
                </div>
                <div id="lblScheduleEmpty" runat="server" class="empty-text" visible="false" style="padding: 12px 16px;">Belum ada jadwal assignment untuk periode ini.</div>
                <div class="legend">
                    <div class="legend-title">Keterangan warna jadwal</div>
                    <div class="legend-item"><span class="legend-dot st-av">AV</span> Available</div>
                    <div class="legend-item"><span class="legend-dot st-av day-today-status">AV</span> Available hari ini (biru muda)</div>
                    <div class="legend-item"><span class="legend-dot st-unit-open">1</span> Unit di-assign - Remaining JO &gt; 0 (merah muda)</div>
                    <div class="legend-item"><span class="legend-dot st-unit-done">1</span> Unit di-assign - Remaining JO = 0 (biru)</div>
                    <div class="legend-item"><span class="legend-dot st-off">OF</span> Off / Libur</div>
                    <div class="legend-item"><span class="legend-dot st-cuti">CT</span> Cuti</div>
                    <div class="legend-item"><span class="legend-dot st-izin">IZ</span> Izin</div>
                    <div class="legend-item"><span class="legend-dot st-unit-open">*</span> Tanggal lewat dan Remaining JO masih ada</div>
                    <div class="legend-note">Angka pada sel menunjukkan jumlah unit yang di-assign. Merah muda berarti masih ada Remaining JO. Biru berarti Remaining JO sudah 0 (semua JO selesai). Biru muda hanya untuk AV hari ini.</div>
                </div>
            </div>
        </div>

        <div class="section-box perf-chart-section">
            <div class="section-header">
                <h4 class="section-title">Grafik Kinerja <span id="lblPerfChartTabTitle">IT Support</span></h4>
                <div class="perf-chart-controls">
                    <select id="assignPerfTechSelect" class="perf-chart-select" aria-label="Pilih IT Support">
                        <option value="">Memuat IT Support...</option>
                    </select>
                </div>
            </div>
            <div class="perf-chart-note-wrap">
                <p class="perf-chart-note" id="assignPerfChartNote">Grafik kinerja IT Support mengikuti periode dashboard. Penugasan harian dari trx_job_assign_detail; total closed Training/Visit dari job_training + TechnicianID assignment.</p>
            </div>
            <div class="perf-chart-grid">
                <div class="perf-chart-panel">
                    <div class="perf-chart-panel-top">
                        <div class="perf-chart-panel-heading">
                            <h5 class="perf-chart-panel-title">Kinerja Harian</h5>
                            <div class="perf-daily-week-nav">
                                <button type="button" id="assignPerfDailyWeekPrev" class="perf-daily-week-btn" title="Minggu sebelumnya" aria-label="Minggu sebelumnya">&lsaquo;</button>
                                <button type="button" id="assignPerfDailyWeekNext" class="perf-daily-week-btn" title="Minggu berikutnya" aria-label="Minggu berikutnya">&rsaquo;</button>
                            </div>
                        </div>
                        <p class="perf-chart-panel-sub" id="assignPerfDailySubtitle">7 hari terakhir pada periode terpilih</p>
                    </div>
                    <div id="assignPerfDailyChart" class="perf-chart-canvas" role="img" aria-label="Grafik kinerja harian IT Support"></div>
                </div>
                <div class="perf-chart-panel">
                    <h5 class="perf-chart-panel-title">Kinerja Bulanan</h5>
                    <p class="perf-chart-panel-sub" id="assignPerfMonthlySubtitle">6 bulan terakhir</p>
                    <div id="assignPerfMonthlyChart" class="perf-chart-canvas" role="img" aria-label="Grafik kinerja bulanan IT Support"></div>
                </div>
            </div>
        </div>

    </section>

    <div class="modal fade" id="modal-detail-jo">
        <div class="modal-dialog modal-lg detail-jo-dialog">
            <div class="modal-content detail-jo-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">
                        Detail Job Order - <span id="lblModalTitle" runat="server">ALL</span>
                    </h4>
                    <small style="color: #7f8d9b;">Periode: <span id="lblModalPeriode" runat="server"></span></small>
                </div>
                <div class="modal-body detail-jo-body">
                    <div class="detail-jo-search-wrap">
                        <input type="text" id="detailJoSearchInput" class="detail-jo-search-input" placeholder="Cari JobID, Customer, JobType, Job Date, Area, Status, OverSLA, Total Unit..." />
                    </div>
                    <div class="table-responsive detail-jo-table-wrap">
                        <table class="table table-bordered table-striped detail-jo-table">
                            <thead>
                                <tr>
                                    <th>No</th>
                                    <th class="detail-jo-sortable" data-sort-col="1" data-sort-type="text">JobID</th>
                                    <th class="detail-jo-sortable" data-sort-col="2" data-sort-type="text">FullName</th>
                                    <th class="detail-jo-sortable" data-sort-col="3" data-sort-type="text">JobType</th>
                                    <th class="detail-jo-sortable" data-sort-col="4" data-sort-type="date">Job Date</th>
                                    <th class="detail-jo-sortable" data-sort-col="5" data-sort-type="date">ScheduleDate</th>
                                    <th class="detail-jo-sortable" data-sort-col="6" data-sort-type="text">Area</th>
                                    <th class="detail-jo-sortable" data-sort-col="7" data-sort-type="number">Total GPS</th>
                                    <th class="detail-jo-sortable" data-sort-col="8" data-sort-type="number">Total ACS</th>
                                    <th class="detail-jo-sortable" data-sort-col="9" data-sort-type="text">Status</th>
                                    <th class="detail-jo-sortable" data-sort-col="10" data-sort-type="number">OverSLA</th>
                                    <th class="detail-jo-sortable" data-sort-col="11" data-sort-type="text">Remark</th>
                                </tr>
                            </thead>
                            <tbody id="detailJoTableBody">
                                <asp:Repeater ID="rptDetailJO" runat="server">
                                    <ItemTemplate>
                                        <tr class='<%# "detail-jo-row " + Convert.ToString(Eval("OverSlaCssClass")) %>'>
                                            <td><%# Container.ItemIndex + 1 %></td>
                                            <td><%# Eval("JobID") %></td>
                                            <td><%# Eval("FullName") %></td>
                                            <td><%# Eval("JobTypeDisplay") %></td>
                                            <td><%# Eval("JobDateDisplay") %></td>
                                            <td><%# Eval("ScheduleDateDisplay") %></td>
                                            <td><%# Eval("AreaDisplay") %></td>
                                            <td><%# Eval("QtyGpsDisplay") %></td>
                                            <td><%# Eval("QtyAcsDisplay") %></td>
                                            <td><%# Eval("Status") %></td>
                                            <td><%# Eval("OverSlaDisplay") %></td>
                                            <td><%# Eval("Remark") %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                            <tfoot>
                                <tr>
                                    <td colspan="7" style="text-align:right;">Total</td>
                                    <td><span id="lblDetailTotalGps" runat="server">0</span></td>
                                    <td><span id="lblDetailTotalAcs" runat="server">0</span></td>
                                    <td colspan="3"></td>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                    <div id="lblDetailEmpty" runat="server" class="empty-text" visible="false">Tidak ada detail JO sesuai filter yang dipilih.</div>
                </div>
                <div class="detail-jo-footer">
                    <button type="button" class="detail-jo-export-btn" onclick="return exportDetailJoToExcel();">Export Excel (.xls)</button>
                    <button type="button" class="detail-jo-close-btn" data-dismiss="modal">Tutup Laporan</button>
                </div>
            </div>
        </div>
    </div>

    <div class="assign-job-backdrop" id="assignJobBackdrop">
        <div class="assign-job-modal" role="dialog" aria-modal="true" aria-labelledby="assignJobTitle">
            <div class="assign-job-modal-header">
                <div class="assign-job-title-wrap">
                    <h4 id="assignJobTitle">Assign IT Support</h4>
                    <p id="assignJobSubtitle">Pilih job order dan status IT Support</p>
                </div>
                <button type="button" class="assign-job-close" id="assignJobCloseBtn" aria-label="Close">&times;</button>
            </div>
            <div class="assign-job-body">
                <div class="assign-info-grid">
                    <div class="assign-info-item">
                        <span class="assign-info-label">IT Support</span>
                        <span class="assign-info-value" id="assignInfoTechName">-</span>
                    </div>
                    <div class="assign-info-item">
                        <span class="assign-info-label">Tanggal</span>
                        <span class="assign-info-value" id="assignInfoDate">-</span>
                    </div>
                    <div class="assign-info-item">
                        <span class="assign-info-label">Status Saat Ini</span>
                        <span class="assign-info-value" id="assignInfoCurrentStatus">-</span>
                    </div>
                </div>

                <div id="assignJobSectionWrap">
                    <p class="assign-section-label">Assign Job</p>
                    <div class="assign-jo-type assign-its-hide-unit" id="assignJobTypeWrap" aria-hidden="true" hidden>
                        <button type="button" class="assign-jo-type-btn active" data-jo-type="installation">JO Training</button>
                        <button type="button" class="assign-jo-type-btn" data-jo-type="maintenance">JO Visit</button>
                    </div>

                    <div class="assign-device-group-block" style="display:none;" aria-hidden="true" hidden>
                        <p class="assign-section-label">Device Group</p>
                        <div class="assign-jo-type" id="assignDeviceGroupWrap">
                            <button type="button" class="assign-jo-type-btn active" data-device-group="GPS">GPS</button>
                            <button type="button" class="assign-jo-type-btn" data-device-group="ACS">ACS</button>
                        </div>
                    </div>

                    <div class="assign-field assign-pick-row">
                        <button type="button" class="assign-pick-btn" id="assignPickJobOrderBtn">Pilih Job Order</button>
                        <div class="assign-picked-jo" id="assignPickedJoText">Belum ada Job Order dipilih</div>
                    </div>

                    <div class="assign-order-card">
                        <div class="assign-info-item">
                            <span class="assign-info-label">Customer</span>
                            <span class="assign-info-value" id="assignInfoCustomer">-</span>
                        </div>
                        <div class="assign-info-item">
                            <span class="assign-info-label">Total Unit Customer</span>
                            <span class="assign-info-value" id="assignInfoRemainingGps">0</span>
                        </div>
                    </div>

                    <div class="assign-field assign-its-hide-unit" id="assignInputUnitWrap">
                        <label for="assignInputUnit" class="assign-field-label" id="assignInputUnitLabel">Assign GPS Unit</label>
                        <input type="number" id="assignInputUnit" min="1" value="1" placeholder="1" />
                        <small class="assign-field-hint" id="assignInputUnitHint">Sudah Assign IT Support : 0</small>
                    </div>
                    <div class="assign-field assign-its-hide-unit" id="assignAreaFieldWrap">
                        <label for="assignAreaSelect" class="assign-field-label">Area</label>
                        <select id="assignAreaSelect">
                            <option value="">Pilih Area</option>
                        </select>
                        <small class="assign-field-hint assign-area-hint" id="assignAreaHint">Pilih area penugasan.</small>
                    </div>
                </div>

                <div class="assign-field" id="assignTransferNoteWrap" hidden>
                    <label for="assignTransferNoteInput" class="assign-field-label">Catatan Pindah IT Support</label>
                    <textarea id="assignTransferNoteInput" class="assign-dashboard-note-textarea" rows="3" placeholder="Tulis alasan pindah IT Support..."></textarea>
                    <small class="assign-field-hint">Wajib diisi saat memindahkan JO ke IT Support lain.</small>
                </div>

                <p class="assign-section-label">Ubah Status</p>
                <div class="assign-status-grid" id="assignStatusWrap">
                    <button type="button" class="assign-status-btn active" data-status-value="AV">Available</button>
                    <button type="button" class="assign-status-btn" data-status-value="OF">Off</button>
                    <button type="button" class="assign-status-btn" data-status-value="CT">Cuti</button>
                    <button type="button" class="assign-status-btn" data-status-value="IZ">Izin</button>
                </div>

                <div class="assign-feedback" id="assignJobFeedback"></div>

            </div>
            <div class="assign-job-footer">
                <div class="assign-footer-left-actions">
                    <button type="button" class="assign-report-close-btn" id="assignJobCloseActionBtn">Tutup Laporan</button>
                    <button type="button" class="assign-action-btn submit-secondary" id="assignJobAdministrationBtn">Is Administration</button>
                </div>
                <div class="assign-actions">
                    <button type="button" class="assign-action-btn cancel" id="assignJobCancelBtn">Cancel</button>
                    <button type="button" class="assign-action-btn submit" id="assignJobSubmitBtn">Assign</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade bs-example-modal-lg" id="modal-training-customer" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Customer</h4>
                </div>
                <div class="modal-body">
                    <iframe src="job_training_customer_search.aspx" style="width: 100%; border: none; height: 350px;" scrolling="no"></iframe>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <div class="assign-job-backdrop" id="assignCreateJoBackdrop">
        <div class="assign-job-modal assign-create-jo-modal" role="dialog" aria-modal="true" aria-labelledby="assignCreateJoTitle">
            <div class="assign-job-modal-header">
                <div class="assign-job-title-wrap">
                    <h4 id="assignCreateJoTitle">Buat Job Training / Visit</h4>
                    <p>Input dan submit JO baru, sama seperti menu Job Training</p>
                </div>
                <button type="button" class="assign-job-close" id="assignCreateJoCloseBtn" aria-label="Close">&times;</button>
            </div>
            <div class="assign-job-body">
                <div class="assign-create-jo-grid">
                    <div class="assign-create-jo-col">
                        <p class="assign-section-label">Customer Information</p>
                        <div class="assign-field">
                            <label class="assign-field-label" for="assignTrainingCustId">Customer ID</label>
                            <input type="hidden" id="assignTrainingCustId" value="" />
                            <button type="button" class="assign-pick-btn" id="assignCreateJoPickCustomerBtn">Pilih Customer</button>
                            <div class="assign-picked-jo" id="assignPickedCustomerText">Belum ada Customer dipilih</div>
                        </div>
                        <div class="assign-info-item">
                            <span class="assign-info-label">Full Name</span>
                            <span class="assign-info-value" id="assignTrainingCustName">-</span>
                        </div>
                        <div class="assign-info-item" style="margin-top:8px;">
                            <span class="assign-info-label">Customer Type</span>
                            <span class="assign-info-value" id="assignTrainingCustType">-</span>
                        </div>
                        <div class="assign-info-item" style="margin-top:8px;">
                            <span class="assign-info-label">Branch Name</span>
                            <span class="assign-info-value" id="assignTrainingCustBranch">-</span>
                        </div>
                        <div class="assign-field">
                            <label class="assign-field-label" for="assignTrainingPicName">PIC Name</label>
                            <input type="text" id="assignTrainingPicName" readonly="readonly" placeholder="PIC Name" />
                        </div>
                        <div class="assign-field">
                            <label class="assign-field-label" for="assignTrainingPicPhone">PIC Phone</label>
                            <input type="text" id="assignTrainingPicPhone" readonly="readonly" placeholder="PIC Phone" />
                        </div>
                    </div>
                    <div class="assign-create-jo-col">
                        <p class="assign-section-label">Training Information</p>
                        <div class="assign-field">
                            <label class="assign-field-label" for="assignTrainingReqDate">Request Date</label>
                            <input type="date" id="assignTrainingReqDate" />
                        </div>
                        <div class="assign-field">
                            <label class="assign-field-label" for="assignTrainingBillable">Billable</label>
                            <select id="assignTrainingBillable">
                                <option value="">[Select]</option>
                            </select>
                        </div>
                        <div class="assign-field">
                            <label class="assign-field-label" for="assignTrainingCategory">Category</label>
                            <select id="assignTrainingCategory">
                                <option value="">[Select]</option>
                            </select>
                        </div>
                        <div class="assign-field">
                            <label class="assign-field-label" for="assignTrainingSchDateInput">Schedule Date</label>
                            <input type="hidden" id="assignTrainingSchDate" value="" />
                            <input type="date" id="assignTrainingSchDateInput" />
                        </div>
                        <div class="assign-field">
                            <label class="assign-field-label" for="assignTrainingRemark">Remark</label>
                            <textarea id="assignTrainingRemark" class="assign-dashboard-note-textarea" rows="3" placeholder="Remark ..."></textarea>
                        </div>
                        <input type="hidden" id="assignTrainingItUserId" value="" />
                        <input type="hidden" id="assignTrainingItUserName" value="" />
                    </div>
                </div>
                <div class="assign-feedback" id="assignCreateJoFeedback"></div>
            </div>
            <div class="assign-job-footer">
                <div class="assign-footer-left-actions"></div>
                <div class="assign-actions">
                    <button type="button" class="assign-action-btn cancel" id="assignCreateJoCancelBtn">Cancel</button>
                    <button type="button" class="assign-action-btn submit" id="assignCreateJoSubmitBtn">Submit</button>
                </div>
            </div>
        </div>
    </div>

    <div class="assign-job-backdrop" id="assignStatusBackdrop" style="display:none;">
        <div class="assign-job-modal" role="dialog" aria-modal="true" aria-labelledby="assignStatusTitle">
            <div class="assign-job-modal-header">
                <div class="assign-job-title-wrap">
                    <h4 id="assignStatusTitle">Ubah Status IT Support</h4>
                    <p id="assignStatusSubtitle">Pilih status terbaru IT Support</p>
                </div>
                <button type="button" class="assign-job-close" id="assignStatusCloseBtn" aria-label="Close">&times;</button>
            </div>
            <div class="assign-job-body">
                <div class="assign-info-grid">
                    <div class="assign-info-item">
                        <span class="assign-info-label">IT Support</span>
                        <span class="assign-info-value" id="assignStatusInfoTechName">-</span>
                    </div>
                    <div class="assign-info-item">
                        <span class="assign-info-label">Tanggal</span>
                        <span class="assign-info-value" id="assignStatusInfoDate">-</span>
                    </div>
                    <div class="assign-info-item">
                        <span class="assign-info-label">Status Saat Ini</span>
                        <span class="assign-info-value" id="assignStatusInfoCurrentStatus">-</span>
                    </div>
                </div>

                <p class="assign-section-label">Ubah Status</p>
                <div class="assign-status-grid" id="assignStatusOnlyWrap">
                    <button type="button" class="assign-status-btn" data-status-value="AV">Available</button>
                    <button type="button" class="assign-status-btn" data-status-value="OF">Off</button>
                    <button type="button" class="assign-status-btn" data-status-value="CT">Cuti</button>
                    <button type="button" class="assign-status-btn" data-status-value="IZ">Izin</button>
                </div>

                <div class="assign-feedback" id="assignStatusFeedback"></div>
            </div>
            <div class="assign-job-footer">
                <div class="assign-footer-left-actions">
                    <button type="button" class="assign-report-close-btn" id="assignStatusCloseActionBtn">Tutup</button>
                </div>
                <div class="assign-actions">
                    <button type="button" class="assign-action-btn cancel" id="assignStatusCancelBtn">Cancel</button>
                    <button type="button" class="assign-action-btn submit" id="assignStatusSubmitBtn">Ubah Status</button>
                </div>
            </div>
        </div>
    </div>

    <div class="assign-job-backdrop assign-jo-info-backdrop" id="assignJoInfoBackdrop">
        <div class="assign-jo-info-modal" role="dialog" aria-modal="true" aria-labelledby="assignJoInfoTitle">
            <div class="assign-job-modal-header">
                <div class="assign-job-title-wrap">
                    <h4 id="assignJoInfoTitle">Job Order Information</h4>
                    <p>Cari dan pilih job order untuk assignment</p>
                </div>
                <button type="button" class="assign-job-close" id="assignJoInfoCloseBtn" aria-label="Close">&times;</button>
            </div>
            <div class="assign-job-body">
                <div class="assign-jo-search-wrap">
                    <input type="text" id="assignJoSearchInput" placeholder="Cari Job ID / Customer / Branch / Remark..." />
                    <button type="button" id="assignJoSearchBtn" aria-label="Search"><i class="fa fa-search"></i></button>
                </div>
                <p class="assign-jo-search-hint">JO yang sudah di-assign tersembunyi. Ketik <strong>Job ID tepat</strong> untuk pindahkan ke IT Support lain.</p>
                <div class="assign-jo-filter-wrap">
                    <label for="assignJoBranchFilter">Branch</label>
                    <select id="assignJoBranchFilter">
                        <option value="">SEMUA Branch</option>
                    </select>
                </div>
                <div class="assign-jo-table-wrap">
                    <table class="assign-jo-table">
                        <thead>
                            <tr>
                                <th>Job Order</th>
                                <th>Customer</th>
                                <th>Branch Name</th>
                                <th>Alamat</th>
                                <th>PIC Customer</th>
                                <th>Customer Number</th>
                                <th>Marketing</th>
                                <th>Remark</th>
                                <th class="assign-jo-device-type-col">Category</th>
                                <th>Total Unit Customer</th>
                                <th title="Tanggal assign terakhir customer yang sudah close">Last Assign</th>
                                <th>Tanggal Create JO</th>
                                <th>SLA Days</th>
                                <th>Action</th>
                            </tr>
                        </thead>
                        <tbody id="assignJoTableBody"></tbody>
                    </table>
                </div>
                <div class="assign-jo-pagination">
                    <button type="button" id="assignJoPrevBtn">Previous</button>
                    <div id="assignJoPageNumbers"></div>
                    <button type="button" id="assignJoNextBtn">Next</button>
                </div>
            </div>
        </div>
    </div>

    <div class="assign-job-backdrop assign-report-backdrop" id="assignReportBackdrop">
        <div class="assign-report-modal" role="dialog" aria-modal="true" aria-labelledby="assignReportTitle">
            <div class="assign-report-header">
                <div class="assign-report-icon" aria-hidden="true">&#10003;</div>
                <div class="assign-report-title-wrap">
                    <h4 id="assignReportTitle">Laporan JO Selesai</h4>
                    <p id="assignReportSubtitle">Detail pekerjaan selesai IT Support</p>
                </div>
                <button type="button" class="assign-report-close" id="assignReportCloseBtn" aria-label="Close">&times;</button>
            </div>
            <div class="assign-report-body">
                <div class="assign-report-feedback" id="assignReportFeedback"></div>
                <div class="assign-report-summary">
                    <div class="assign-info-item">
                        <span class="assign-info-label">IT Support</span>
                        <span class="assign-info-value" id="assignReportTechName">-</span>
                    </div>
                    <div class="assign-info-item">
                        <span class="assign-info-label">Tanggal</span>
                        <span class="assign-info-value" id="assignReportDate">-</span>
                    </div>
                    <div class="assign-info-item">
                        <span class="assign-info-label">Total Unit Selesai</span>
                        <span class="assign-info-value" id="assignReportTotalUnit">0</span>
                    </div>
                    <div class="assign-info-item">
                        <span class="assign-info-label">Total Unit Dalam Proses</span>
                        <span class="assign-info-value" id="assignReportTotalUnitBelumSelesai">0</span>
                    </div>
                </div>

                <p class="assign-section-label">Detail Informasi</p>
                <div class="assign-report-table-wrap">
                    <table class="assign-report-table">
                        <thead>
                            <tr>
                                <th>Action</th>
                                <th>Status</th>
                                <th>FullName</th>
                                <th>JobID</th>
                                <th>SchDate</th>
                                <th>Tanggal Assign</th>
                                <th>Alamat</th>
                                <th>PIC Customer</th>
                                <th>Dikerjakan Oleh</th>
                            </tr>
                        </thead>
                        <tbody id="assignReportTableBody"></tbody>
                    </table>
                </div>

                <div class="assign-report-delete-card" id="assignReportDeleteCard" hidden>
                    <p class="assign-section-label">Hapus Assign</p>
                    <p class="assign-report-delete-summary" id="assignReportDeleteSummary">-</p>
                    <div class="assign-field">
                        <label for="assignReportDeleteNoteInput" class="assign-field-label">Catatan Hapus Assign</label>
                        <textarea id="assignReportDeleteNoteInput" class="assign-dashboard-note-textarea" rows="3" placeholder="Tulis alasan penghapusan..."></textarea>
                        <small class="assign-field-hint">Wajib diisi sebelum menghapus assign.</small>
                    </div>
                    <div class="assign-report-inline-actions">
                        <button type="button" class="assign-action-btn cancel" id="assignReportDeleteCancelBtn">Batal</button>
                        <button type="button" class="assign-report-submit-btn assign-report-delete-submit-btn" id="assignReportDeleteConfirmBtn">Hapus</button>
                    </div>
                </div>

                <div class="assign-report-assign-card" id="assignReportAssignCard">
                    <div class="assign-report-assign-intro" id="assignReportAssignIntro">
                        <p class="assign-section-label">Assign Job</p>
                        <p class="assign-report-assign-hint">Klik tombol di bawah untuk menambah job order baru pada tanggal ini.</p>
                        <button type="button" class="assign-report-add-btn" id="assignReportAddBtn">+ Tambah Assign</button>
                    </div>
                    <div class="assign-report-assign-form" id="assignReportAssignForm" hidden>
                    <p class="assign-section-label">Assign Job</p>
                    <div class="assign-jo-type assign-its-hide-unit" id="assignReportJobTypeWrap" aria-hidden="true" hidden>
                        <button type="button" class="assign-jo-type-btn active" data-jo-type="installation">JO Training</button>
                        <button type="button" class="assign-jo-type-btn" data-jo-type="maintenance">JO Visit</button>
                    </div>

                    <div class="assign-device-group-block" style="display:none;" aria-hidden="true" hidden>
                        <p class="assign-section-label">Device Group</p>
                        <div class="assign-jo-type" id="assignReportDeviceGroupWrap">
                            <button type="button" class="assign-jo-type-btn active" data-device-group="GPS">GPS</button>
                            <button type="button" class="assign-jo-type-btn" data-device-group="ACS">ACS</button>
                        </div>
                    </div>

                    <div class="assign-field assign-pick-row">
                        <button type="button" class="assign-pick-btn" id="assignReportPickJobOrderBtn">Pilih Job Order</button>
                        <div class="assign-picked-jo" id="assignReportPickedJoText">Belum ada Job Order dipilih</div>
                    </div>

                    <div class="assign-order-card">
                        <div class="assign-info-item">
                            <span class="assign-info-label">Customer</span>
                            <span class="assign-info-value" id="assignReportInfoCustomer">-</span>
                        </div>
                        <div class="assign-info-item">
                            <span class="assign-info-label">Total Unit Customer</span>
                            <span class="assign-info-value" id="assignReportRemainingGps">0</span>
                        </div>
                    </div>

                    <div class="assign-field assign-its-hide-unit" id="assignReportInputUnitWrap">
                        <label for="assignReportInputUnit" class="assign-field-label" id="assignReportInputUnitLabel">Assign GPS Unit</label>
                        <input type="number" id="assignReportInputUnit" min="1" value="1" placeholder="Input assign GPS unit" />
                        <small class="assign-field-hint" id="assignReportInputUnitHint">Sudah Assign IT Support : 0</small>
                    </div>
                    <div class="assign-field assign-its-hide-unit" id="assignReportAreaFieldWrap">
                        <label for="assignReportAreaSelect" class="assign-field-label">Area</label>
                        <select id="assignReportAreaSelect">
                            <option value="">Pilih Area</option>
                        </select>
                        <small class="assign-field-hint assign-area-hint" id="assignReportAreaHint">Pilih area penugasan.</small>
                    </div>
                    <div class="assign-field" id="assignReportTransferNoteWrap" hidden>
                        <label for="assignReportTransferNoteInput" class="assign-field-label">Catatan Pindah IT Support</label>
                        <textarea id="assignReportTransferNoteInput" class="assign-dashboard-note-textarea" rows="3" placeholder="Tulis alasan pindah IT Support..."></textarea>
                        <small class="assign-field-hint">Wajib diisi saat memindahkan JO ke IT Support lain.</small>
                    </div>

                    <p class="assign-section-label">Ubah Status</p>
                    <div class="assign-status-grid" id="assignReportStatusWrap">
                        <button type="button" class="assign-status-btn active" data-status-value="AV">Available</button>
                        <button type="button" class="assign-status-btn" data-status-value="OF">Off</button>
                        <button type="button" class="assign-status-btn" data-status-value="CT">Cuti</button>
                        <button type="button" class="assign-status-btn" data-status-value="IZ">Izin</button>
                    </div>

                    <div class="assign-report-inline-actions">
                        <button type="button" class="assign-action-btn cancel" id="assignReportCancelAddBtn">Batal</button>
                        <button type="button" class="assign-report-submit-btn" id="assignReportAssignBtn">Assign</button>
                    </div>
                    </div>
                </div>

            </div>
            <div class="assign-report-actions">
                <button type="button" class="assign-report-close-btn" id="assignReportCloseActionBtn">Tutup Laporan</button>
            </div>
        </div>
    </div>

    <div id="assignToast" class="assign-toast" role="status" aria-live="polite"></div>

    <div class="modal fade assign-closed-modal" id="assignClosedJobModal" tabindex="-1" role="dialog" aria-labelledby="assignTotalJobTitle">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <button type="button" class="close assign-closed-modal-close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <div class="assign-closed-modal-top">
                    <div class="assign-closed-eyebrow" id="assignClosedJobEyebrow">Daftar Pekerjaan Diselesaikan</div>
                    <h4 id="assignTotalJobTitle">-</h4>
                </div>
                <div class="assign-closed-divider"></div>
                <div class="modal-body assign-closed-modal-body">
                    <div class="assign-totaljob-summary-wrap">
                        <div class="assign-totaljob-summary assign-totaljob-summary--jo">
                            <span class="assign-totaljob-summary-label">Total Closed JO Training Bulan Ini</span>
                            <strong id="assignTotalJobCount">0</strong>
                        </div>
                        <div class="assign-totaljob-summary assign-totaljob-summary--unit">
                            <span class="assign-totaljob-summary-label">Total Closed JO Visit Bulan Ini</span>
                            <strong id="assignTotalUnitCount">0</strong>
                        </div>
                    </div>
                    <div id="assignTotalJobContent"></div>
                    <div class="assign-closed-actions">
                        <button type="button" class="assign-closed-close-btn" data-dismiss="modal">Tutup Laporan</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade assign-closed-modal" id="assignDayTotalJoModal" tabindex="-1" role="dialog" aria-labelledby="assignDayTotalJoTitle">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <button type="button" class="close assign-closed-modal-close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <div class="assign-closed-modal-top">
                    <div class="assign-closed-eyebrow">Total JO per Tanggal</div>
                    <h4 id="assignDayTotalJoTitle">-</h4>
                </div>
                <div class="assign-closed-divider"></div>
                <div class="modal-body assign-closed-modal-body">
                    <div class="assign-totaljob-summary-wrap">
                        <div class="assign-totaljob-summary assign-totaljob-summary--jo">
                            <span class="assign-totaljob-summary-label">Total JO</span>
                            <strong id="assignDayTotalJoCount">0</strong>
                        </div>
                        <div class="assign-totaljob-summary assign-totaljob-summary--unit">
                            <span class="assign-totaljob-summary-label">Total Unit</span>
                            <strong id="assignDayTotalJoUnitCount">0</strong>
                        </div>
                    </div>
                    <div id="assignDayTotalJoContent"></div>
                    <div class="assign-closed-actions">
                        <button type="button" class="assign-closed-close-btn" data-dismiss="modal">Tutup</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="assign-tech-modal-backdrop" id="assignTechStockBackdrop" style="display:none !important;" aria-hidden="true" hidden>
        <div class="assign-tech-modal" role="dialog" aria-modal="true" aria-labelledby="assignTechStockTitle">
            <div class="assign-tech-modal-head">
                <div>
                    <p>Batch Stock Alat IT Support</p>
                    <h4 id="assignTechStockTitle">-</h4>
                </div>
                <button type="button" class="assign-tech-modal-close" id="assignTechStockCloseBtn" aria-label="Close">&times;</button>
            </div>
            <div id="assignTechStockContent"></div>
            <div class="assign-tech-modal-footer">
                <button type="button" class="assign-tech-modal-btn" id="assignTechStockCloseActionBtn">Tutup</button>
            </div>
        </div>
    </div>

    <div class="assign-tech-detail-backdrop" id="assignTechDeviceDetailBackdrop" style="display:none !important;" aria-hidden="true" hidden>
        <div class="assign-tech-detail-modal" role="dialog" aria-modal="true" aria-labelledby="assignTechDeviceDetailTitle">
            <div class="assign-tech-detail-head">
                <div>
                    <h4 id="assignTechDeviceDetailTitle">Detail Device IT Support</h4>
                    <p id="assignTechDeviceDetailSubtitle">-</p>
                </div>
                <button type="button" class="assign-tech-detail-close" id="assignTechDeviceDetailCloseBtn" aria-label="Close">&times;</button>
            </div>
            <div class="assign-tech-detail-body">
                <div class="assign-tech-detail-search">
                    <input type="text" id="assignTechDeviceSearchInput" placeholder="Cari Device ID, SN, Device Type, Status..." />
                </div>
                <div class="assign-tech-detail-feedback" id="assignTechDeviceDetailFeedback"></div>
                <div class="assign-tech-detail-table-wrap">
                    <table class="assign-tech-detail-table">
                        <thead>
                            <tr>
                                <th>Device ID</th>
                                <th>No SN</th>
                                <th>Device Type</th>
                                <th>Status</th>
                            </tr>
                        </thead>
                        <tbody id="assignTechDeviceDetailTableBody"></tbody>
                    </table>
                </div>
            </div>
            <div class="assign-tech-detail-footer">
                <div class="assign-tech-detail-legend">
                    <div class="assign-tech-detail-legend-item">
                        <span class="assign-tech-detail-status ready">Ready</span>
                        <span>= Siap Pakai</span>
                    </div>
                    <div class="assign-tech-detail-legend-item">
                        <span class="assign-tech-detail-status">MQ</span>
                        <span>= Alat Cabutan (Belum QC, Serahkan ke Admin IT Support)</span>
                    </div>
                    <div class="assign-tech-detail-legend-item">
                        <span class="assign-tech-detail-status">BR</span>
                        <span>= Alat Cabutan Rusak (Perlu Perbaikan)</span>
                    </div>
                </div>
                <button type="button" class="assign-tech-modal-btn" id="assignTechDeviceDetailCloseActionBtn">Tutup</button>
            </div>
        </div>
    </div>

    <div class="assign-tech-detail-backdrop assign-report-remark-backdrop" id="assignReportRemarkBackdrop">
        <div class="assign-report-remark-modal" role="dialog" aria-modal="true" aria-labelledby="assignReportRemarkTitle">
            <div class="assign-report-remark-head">
                <div>
                    <h4 id="assignReportRemarkTitle">Detail Remark</h4>
                    <p id="assignReportRemarkSubtitle">Laporan JO Selesai</p>
                </div>
                <button type="button" class="assign-report-remark-close" id="assignReportRemarkCloseBtn" aria-label="Close">&times;</button>
            </div>
            <div class="assign-report-remark-body">
                <p class="assign-report-remark-content" id="assignReportRemarkContent">-</p>
            </div>
            <div class="assign-report-remark-footer">
                <button type="button" class="assign-tech-modal-btn" id="assignReportRemarkCloseActionBtn">Tutup</button>
            </div>
        </div>
    </div>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/echarts/5.4.3/echarts.min.js"></script>
    <script type="text/javascript">
        (function () {
            var assignModalState = {
                activeCell: null,
                joType: "new_install",
                deviceGroupId: "GPS",
                targetStatus: "AV",
                administrationTargetStatus: "AD",
                supAreaId: "",
                selectedAreaId: "",
                selectedOrder: null,
                joPage: 1,
                joSearchText: "",
                joBranchFilter: "",
                joBranchOptions: [],
                joRows: [],
                joTotalPages: 1,
                joTotalRecords: 0,
                joLoading: false,
                isSaving: false,
                submitAction: "assign"
            };
            var statusOnlyModalState = {
                activeCell: null,
                technicianId: "",
                technicianName: "",
                schDate: "",
                currentStatus: "",
                targetStatus: "",
                isSaving: false
            };
            var reportModalState = {
                activeCell: null,
                technicianId: "",
                technicianName: "",
                schDate: "",
                joType: "new_install",
                deviceGroupId: "GPS",
                targetStatus: "AV",
                supAreaId: "",
                selectedAreaId: "",
                selectedOrder: null,
                editingRowIndex: -1,
                rows: [],
                totalUnitSelesai: 0,
                totalUnitBelumSelesai: 0,
                isSaving: false,
                isLoading: false,
                submitAction: "assign",
                requestToken: 0,
                showAssignForm: false,
                pendingDeleteRowIndex: -1
            };
            var areaLookupState = {
                rows: [],
                loaded: false,
                loading: false,
                supAreaId: ""
            };
            var assignDocumentEventsBound = false;
            var assignJoPageSize = 20;
            var assignNewInstallPageUrl = "<%= ResolveUrl("~/installation_job_new.aspx") %>";
            var closedJobRequestToken = 0;
            var dayTotalJoRequestToken = 0;
            var joLookupOwner = "main";
            var techStockState = {
                technicianId: "",
                technicianName: "",
                gpsDevices: [],
                gsmItems: [],
                accessories: [],
                totalUnit: 0
            };
            var techDeviceDetailState = {
                technicianId: "",
                technicianName: "",
                deviceTypeId: "",
                deviceTypeDesc: "",
                rows: [],
                searchText: ""
            };
            var techDeviceDetailCache = {};
            var technicianFlagFieldId = "<%= hfIsTechnician.ClientID %>";

            function getIsTechnicianUser() {
                var field = document.getElementById(technicianFlagFieldId);
                return !!(field && field.value === "1");
            }

            function setActiveTabUI(tab) {
                var tabs = document.querySelectorAll(".assign-tab-btn");
                for (var i = 0; i < tabs.length; i++) {
                    var button = tabs[i];
                    if ((button.getAttribute("data-tab") || "").toLowerCase() === tab) {
                        button.classList.add("active");
                    } else {
                        button.classList.remove("active");
                    }
                }
            }

            function normalizeTab(tab) {
                var value = (tab || "").toLowerCase();
                return value === "itsupport" ? "itsupport" : "teknisi";
            }

            function parseIsoDate(dateText) {
                var parts = (dateText || "").split("-");
                if (parts.length !== 3) {
                    return null;
                }
                var year = parseInt(parts[0], 10);
                var month = parseInt(parts[1], 10) - 1;
                var day = parseInt(parts[2], 10);
                if (isNaN(year) || isNaN(month) || isNaN(day)) {
                    return null;
                }
                return new Date(year, month, day);
            }

            function getTodayDateOnly() {
                var now = new Date();
                return new Date(now.getFullYear(), now.getMonth(), now.getDate());
            }

            function isPastScheduleDate(dateText) {
                var scheduleDate = parseIsoDate(dateText);
                if (!scheduleDate) {
                    return false;
                }
                return scheduleDate.getTime() < getTodayDateOnly().getTime();
            }

            function syncTodayCellClasses(cell, value) {
                if (!cell) {
                    return;
                }

                var td = cell.closest("td");
                var cellDate = parseIsoDate(cell.getAttribute("data-date"));
                var isToday = !!cellDate && cellDate.getTime() === getTodayDateOnly().getTime();
                var normalizedValue = (value || "").toString().trim().toUpperCase();
                var isTodayAv = isToday && (normalizedValue === "AV" || normalizedValue === "AD");

                cell.classList.toggle("day-today-status", isTodayAv);
                if (td) {
                    td.classList.toggle("day-today-cell", isTodayAv);
                }
            }

            function formatDisplayDate(dateText) {
                var date = parseIsoDate(dateText);
                if (!date) {
                    return dateText || "-";
                }
                return date.toLocaleDateString("id-ID", {
                    day: "2-digit",
                    month: "short",
                    year: "numeric"
                });
            }

            function closestByClass(element, className) {
                var current = element;
                while (current && current !== document) {
                    if (current.classList && current.classList.contains(className)) {
                        return current;
                    }
                    current = current.parentNode;
                }
                return null;
            }

            function getBackdrop() {
                return document.getElementById("assignJobBackdrop");
            }

            function getStatusOnlyBackdrop() {
                return document.getElementById("assignStatusBackdrop");
            }

            function getCompletedReportBackdrop() {
                return document.getElementById("assignReportBackdrop");
            }

            function getReportRemarkBackdrop() {
                return document.getElementById("assignReportRemarkBackdrop");
            }

            function getDashboardNoteValue(inputId) {
                var input = document.getElementById(inputId);
                return input ? (input.value || "").toString().trim() : "";
            }

            function clearDashboardNote(inputId) {
                var input = document.getElementById(inputId);
                if (input) {
                    input.value = "";
                }
            }

            function normalizeTechId(value) {
                return (value || "").toString().trim().toUpperCase();
            }

            function getOrderRemainingUnit(order) {
                if (!order) {
                    return 0;
                }
                var legacy = toInt(order.RemainingUnit, -1);
                if (legacy >= 0) {
                    return legacy;
                }
                return toInt(order.RemainingUnitGps, 0) + toInt(order.RemainingUnitAcs, 0);
            }

            function getOrderAssignedTechnicianId(order) {
                if (!order) {
                    return "";
                }
                return (order.AssignedTechnicianId || order.AssignedTechnicianID || "").toString().trim();
            }

            function getOrderAssignedSchDate(order) {
                if (!order) {
                    return "";
                }
                return normalizeScheduleDateText(order.AssignedSchDate || order.AssignedScheduleDate || "");
            }

            function normalizeScheduleDateText(value) {
                var text = (value || "").toString().trim();
                if (!text) {
                    return "";
                }
                var parsed = parseIsoDate(text);
                if (parsed) {
                    var month = parsed.getMonth() + 1;
                    var day = parsed.getDate();
                    return parsed.getFullYear()
                        + "-" + (month < 10 ? "0" : "") + month
                        + "-" + (day < 10 ? "0" : "") + day;
                }
                if (/^\d{4}-\d{2}-\d{2}/.test(text)) {
                    return text.substring(0, 10);
                }
                return text;
            }

            function sameScheduleDate(left, right) {
                var a = normalizeScheduleDateText(left);
                var b = normalizeScheduleDateText(right);
                return !!a && a === b;
            }

            function addAssignRefreshTarget(targets, dateText, technicianId) {
                var date = normalizeScheduleDateText(dateText);
                if (!date) {
                    return;
                }

                var existing = null;
                for (var i = 0; i < targets.length; i++) {
                    if (targets[i].date === date) {
                        existing = targets[i];
                        break;
                    }
                }
                if (!existing) {
                    existing = { date: date, techIds: [] };
                    targets.push(existing);
                }

                var techId = (technicianId || "").toString().trim();
                if (techId && existing.techIds.indexOf(techId) < 0) {
                    existing.techIds.push(techId);
                }
            }

            function buildAssignRefreshTargets(primaryDate, technicianIds, previousDate, previousTechnicianId) {
                var targets = [];
                var ids = technicianIds || [];
                if (ids.length) {
                    for (var i = 0; i < ids.length; i++) {
                        addAssignRefreshTarget(targets, primaryDate, ids[i]);
                    }
                } else {
                    addAssignRefreshTarget(targets, primaryDate, "");
                }
                addAssignRefreshTarget(targets, previousDate, previousTechnicianId);
                return targets;
            }

            function refreshAffectedScheduleDays(targets, primaryCell, callback) {
                var list = targets || [];
                function next(index) {
                    if (index >= list.length) {
                        if (typeof callback === "function") {
                            callback();
                        }
                        return;
                    }

                    var item = list[index] || {};
                    var cell = null;
                    if (primaryCell && sameScheduleDate(primaryCell.getAttribute("data-date"), item.date)) {
                        cell = primaryCell;
                    } else if (item.techIds && item.techIds.length) {
                        cell = findScheduleCell(item.techIds[0], item.date);
                    }

                    refreshScheduleDayState(item.date, item.techIds || [], cell, function () {
                        next(index + 1);
                    });
                }
                next(0);
            }

            function refreshAfterAssignChange(options, callback) {
                options = options || {};
                refreshAffectedScheduleDays(
                    buildAssignRefreshTargets(
                        options.scheduleDate,
                        options.technicianIds,
                        options.previousSchDate,
                        options.previousTechnicianId),
                    options.primaryCell || null,
                    callback);
            }

            function getTargetAssignTechnicianId(context) {
                if (context === "report") {
                    return (reportModalState.technicianId || "").trim();
                }
                var cell = assignModalState.activeCell;
                return cell ? (cell.getAttribute("data-tech-id") || "").trim() : "";
            }

            function isJobOrderTransfer(order, targetTechnicianId) {
                if (!order) {
                    return false;
                }

                var targetId = normalizeTechId(targetTechnicianId);
                var assignedId = normalizeTechId(getOrderAssignedTechnicianId(order));

                if (assignedId && targetId && assignedId !== targetId) {
                    return true;
                }

                if (order.IsTransfer === true || order.IsTransfer === "true" || order.IsTransfer === 1) {
                    if (assignedId && targetId) {
                        return assignedId !== targetId;
                    }
                    return toInt(order.TotalAssign, 0) > 0;
                }

                return toInt(order.TotalAssign, 0) > 0
                    && getOrderRemainingUnit(order) <= 0
                    && assignedId
                    && targetId
                    && assignedId !== targetId;
            }

            function isJobOrderTransferCandidate(order) {
                if (!order) {
                    return false;
                }
                if (order.IsTransfer === true || order.IsTransfer === "true" || order.IsTransfer === 1) {
                    return true;
                }
                if (normalizeTechId(getOrderAssignedTechnicianId(order))) {
                    return true;
                }
                return toInt(order.TotalAssign, 0) > 0 && getOrderRemainingUnit(order) <= 0;
            }

            function requiresTransferNoteForOrder(order, targetTechnicianId) {
                if (!order) {
                    return false;
                }
                if (order._requiresTransferNote === true) {
                    return true;
                }
                return isJobOrderTransfer(order, targetTechnicianId);
            }

            function enrichSelectedOrderAssignContext(order, context, callback) {
                if (!order || !order.JobID) {
                    if (typeof callback === "function") {
                        callback(order);
                    }
                    return;
                }

                var techId = getTargetAssignTechnicianId(context);
                callAssignPageMethod(
                    "GetJobOrderAssignContext",
                    {
                        jobId: order.JobID || "",
                        technicianId: techId
                    },
                    function (result) {
                        var enriched = order;
                        if (result && (result.Result || "").toUpperCase() === "SUCCESS") {
                            enriched._requiresTransferNote = !!result.RequiresTransferNote;
                            enriched.IsAlreadyAssigned = !!result.IsAlreadyAssigned;
                            if (result.AssignedTechnicianId) {
                                enriched.AssignedTechnicianId = result.AssignedTechnicianId;
                            }
                            if (result.AssignedTechnicianName) {
                                enriched.AssignedTechnicianName = result.AssignedTechnicianName;
                            }
                            if (result.AssignedSchDate) {
                                enriched.AssignedSchDate = result.AssignedSchDate;
                            }
                            if (enriched._requiresTransferNote || enriched.IsAlreadyAssigned) {
                                enriched.IsTransfer = true;
                            }
                        }
                        if (typeof callback === "function") {
                            callback(enriched);
                        }
                    },
                    function () {
                        if (typeof callback === "function") {
                            callback(order);
                        }
                    });
            }

            function applySelectedJobOrder(order, context) {
                enrichSelectedOrderAssignContext(order, context, function (enriched) {
                    var pickedJoType = "new_install";
                    if (enriched && (enriched.DeviceTypeDesc || "").toString().toLowerCase().indexOf("visit") >= 0) {
                        pickedJoType = "maintenance";
                    }
                    if (context === "report") {
                        reportModalState.showAssignForm = true;
                        reportModalState.joType = pickedJoType;
                        reportModalState.selectedOrder = enriched;
                        reportModalState.editingRowIndex = -1;
                        applySelectedOrderAreaFromCustomer(enriched, reportModalState);
                        renderReportAssignForm();
                        if (requiresTransferNoteForOrder(enriched, reportModalState.technicianId)) {
                            setReportFeedback(getJobOrderTransferLabel(enriched, reportModalState.technicianId) || "Isi catatan pindah IT Support sebelum assign.", false, false);
                        } else {
                            setReportFeedback("", false, false);
                        }
                    } else {
                        assignModalState.joType = pickedJoType;
                        assignModalState.selectedOrder = enriched;
                        applySelectedOrderAreaFromCustomer(enriched, assignModalState);
                        renderOrderInfo();
                        if (requiresTransferNoteForOrder(enriched, getTargetAssignTechnicianId("main"))) {
                            setFeedback(getJobOrderTransferLabel(enriched, getTargetAssignTechnicianId("main")) || "Isi catatan pindah IT Support sebelum assign.", false, false);
                        } else {
                            setFeedback("", false, false);
                        }
                    }
                });
            }

            function syncTransferNoteField(wrapId, inputId, order, targetTechnicianId) {
                var wrap = document.getElementById(wrapId);
                var show = requiresTransferNoteForOrder(order, targetTechnicianId);
                if (wrap) {
                    wrap.hidden = !show;
                    wrap.style.display = show ? "" : "none";
                }
                if (!show) {
                    clearDashboardNote(inputId);
                }
            }

            function renderReportDeletePanel() {
                var card = document.getElementById("assignReportDeleteCard");
                var assignCard = document.getElementById("assignReportAssignCard");
                var rowIndex = reportModalState.pendingDeleteRowIndex;
                var show = rowIndex >= 0;
                var rows = reportModalState.rows || [];
                var row = show ? (rows[rowIndex] || {}) : null;
                var summary = document.getElementById("assignReportDeleteSummary");

                if (card) {
                    card.hidden = !show;
                    card.style.display = show ? "" : "none";
                }
                if (assignCard) {
                    if (show) {
                        assignCard.style.display = "none";
                    } else if (!getIsTechnicianUser() && !isPastScheduleDate(reportModalState.schDate || "")) {
                        assignCard.style.display = "";
                    }
                }
                if (summary && row) {
                    summary.textContent = (row.JobID || "-") + " - " + getScheduleRowCustomerName(row);
                }
                if (!show) {
                    clearDashboardNote("assignReportDeleteNoteInput");
                }
            }

            function openReportDeletePanel(rowIndex) {
                reportModalState.pendingDeleteRowIndex = rowIndex;
                reportModalState.showAssignForm = false;
                resetReportAssignFormFields();
                reportModalState.pendingDeleteRowIndex = rowIndex;
                renderReportAssignForm();
                renderReportDeletePanel();
                var deleteInput = document.getElementById("assignReportDeleteNoteInput");
                if (deleteInput) {
                    window.setTimeout(function () {
                        deleteInput.focus();
                    }, 0);
                }
                setReportFeedback("Isi catatan lalu klik Hapus untuk menghapus assign.", false, false);
            }

            function cancelReportDeletePanel() {
                reportModalState.pendingDeleteRowIndex = -1;
                clearDashboardNote("assignReportDeleteNoteInput");
                renderReportDeletePanel();
                renderReportAssignForm();
                setReportFeedback("", false, false);
            }

            function confirmReportDelete() {
                var rowIndex = reportModalState.pendingDeleteRowIndex;
                var rows = reportModalState.rows || [];
                if (rowIndex < 0 || rowIndex >= rows.length) {
                    cancelReportDeletePanel();
                    return;
                }

                var row = rows[rowIndex] || {};
                var actionRemark = getDashboardNoteValue("assignReportDeleteNoteInput");
                if (!actionRemark) {
                    setReportFeedback("Catatan hapus assign wajib diisi.", true, false);
                    var deleteInput = document.getElementById("assignReportDeleteNoteInput");
                    if (deleteInput) {
                        deleteInput.focus();
                    }
                    return;
                }

                if (!row.AssignID) {
                    setReportFeedback("AssignID tidak ditemukan untuk data yang dipilih.", true, false);
                    return;
                }
                if (typeof row.Seq === "undefined" || row.Seq === null || toInt(row.Seq, -1) < 0) {
                    setReportFeedback("Seq tidak ditemukan untuk data yang dipilih.", true, false);
                    return;
                }

                setReportFeedback("Menghapus assign job...", false, false);
                callAssignPageMethod(
                    "DeleteScheduleAssign",
                    {
                        assignId: row.AssignID || "",
                        seq: toInt(row.Seq, -1),
                        actionRemark: actionRemark
                    },
                    function (result) {
                        var success = (result && result.Result ? result.Result : "").toUpperCase() === "SUCCESS";
                        if (!success) {
                            setReportFeedback((result && result.Message) || "Gagal menghapus assign job.", true, false);
                            return;
                        }

                        reportModalState.pendingDeleteRowIndex = -1;
                        clearDashboardNote("assignReportDeleteNoteInput");
                        setReportFeedback((result && result.Message) || "Assign job berhasil dihapus.", false, true);
                        loadCompletedReportModalData(false);
                        renderReportDeletePanel();
                        refreshAfterAssignChange({
                            scheduleDate: (result && result.SchDate) || reportModalState.schDate,
                            technicianIds: [(result && result.TechnicianId) || reportModalState.technicianId],
                            primaryCell: reportModalState.activeCell
                        }, function () { });
                        if (typeof fetchJobOrderInformation === "function") {
                            fetchJobOrderInformation();
                        }
                    },
                    function (errorMessage) {
                        setReportFeedback("Gagal menghapus assign job. " + (errorMessage || ""), true, false);
                    });
            }

            function isCompletedReportModalOpen() {
                var backdrop = getCompletedReportBackdrop();
                return !!(backdrop && backdrop.classList.contains("open"));
            }

            function getTotalJobBackdrop() {
                return document.getElementById("assignClosedJobModal");
            }

            function getTechStockBackdrop() {
                return document.getElementById("assignTechStockBackdrop");
            }

            function toInt(source, fallback) {
                var parsed = parseInt(source, 10);
                return isNaN(parsed) ? (fallback || 0) : parsed;
            }

            function normalizeAreaId(value) {
                return (value || "").toString().trim();
            }

            function setAreaHint(hintElement, text, state) {
                if (!hintElement) {
                    return;
                }
                hintElement.classList.remove("active");
                hintElement.classList.remove("error");
                if (state === "active") {
                    hintElement.classList.add("active");
                } else if (state === "error") {
                    hintElement.classList.add("error");
                }
                hintElement.textContent = text || "Pilih area penugasan.";
            }

            function renderAreaSelectOptions(selectElement, selectedAreaId) {
                if (!selectElement) {
                    return;
                }

                var selected = normalizeAreaId(selectedAreaId);
                var options = ["<option value=\"\">Pilih Area</option>"];
                var rows = areaLookupState.rows || [];
                for (var i = 0; i < rows.length; i++) {
                    var row = rows[i] || {};
                    var areaId = normalizeAreaId(row.AreaID);
                    if (!areaId) {
                        continue;
                    }
                    var areaName = (row.AreaName || "").toString();
                    var isSelected = areaId === selected;
                    options.push("<option value=\"" + escapeHtml(areaId) + "\" title=\"" + escapeHtml(areaName || areaId) + "\"" + (isSelected ? " selected" : "") + ">" + escapeHtml(areaName || areaId) + "</option>");
                }
                selectElement.innerHTML = options.join("");
            }

            function fetchAreaOptions(supAreaId, onComplete) {
                var requestedSupAreaId = normalizeAreaId(supAreaId);
                if (areaLookupState.loaded && areaLookupState.supAreaId === requestedSupAreaId) {
                    if (typeof onComplete === "function") {
                        onComplete();
                    }
                    return;
                }
                if (areaLookupState.loading && areaLookupState.supAreaId === requestedSupAreaId) {
                    return;
                }

                areaLookupState.loading = true;
                areaLookupState.loaded = false;
                areaLookupState.supAreaId = requestedSupAreaId;
                areaLookupState.rows = [];

                var request = new XMLHttpRequest();
                request.open(
                    "GET",
                    getAssignPageUrl()
                        + "?action=load_area_options"
                        + "&supAreaId=" + encodeURIComponent(requestedSupAreaId || ""),
                    true);
                request.setRequestHeader("Accept", "application/json");
                request.onreadystatechange = function () {
                    if (request.readyState !== 4) {
                        return;
                    }

                    try {
                        var text = String(request.responseText || "").replace(/^\uFEFF/, "").trim();
                        if (!text || text.charAt(0) === "<") {
                            throw new Error("Invalid area response");
                        }
                        var result = JSON.parse(text);
                        if (result && typeof result.d !== "undefined") {
                            result = typeof result.d === "string" ? JSON.parse(result.d) : result.d;
                        }
                        areaLookupState.loading = false;
                        var success = (result && result.Result ? result.Result : "").toUpperCase() === "SUCCESS";
                        areaLookupState.rows = success && result.Rows ? result.Rows : [];
                        areaLookupState.loaded = success;
                    } catch (ex) {
                        areaLookupState.loading = false;
                        areaLookupState.rows = [];
                        areaLookupState.loaded = false;
                    }

                    if (typeof onComplete === "function") {
                        onComplete();
                    }
                };
                request.send(null);
            }

            function syncAssignAreaDropdown() {
                var areaSelect = document.getElementById("assignAreaSelect");
                var areaHint = document.getElementById("assignAreaHint");
                if (!areaSelect) {
                    return;
                }

                if (areaLookupState.loading && !areaLookupState.loaded) {
                    areaSelect.innerHTML = "<option value=\"\">Memuat area...</option>";
                    areaSelect.disabled = true;
                    setAreaHint(areaHint, "Memuat daftar area...", "");
                    return;
                }

                if (!areaLookupState.rows.length) {
                    areaSelect.innerHTML = "<option value=\"\">Area tidak tersedia</option>";
                    areaSelect.disabled = true;
                    setAreaHint(areaHint, "Area tidak tersedia. Coba pilih regional spesifik, atau refresh halaman.", "error");
                    return;
                }

                renderAreaSelectOptions(areaSelect, assignModalState.selectedAreaId);
                if (!normalizeAreaId(assignModalState.selectedAreaId) && areaLookupState.rows.length) {
                    assignModalState.selectedAreaId = normalizeAreaId(areaLookupState.rows[0].AreaID);
                    renderAreaSelectOptions(areaSelect, assignModalState.selectedAreaId);
                }
                areaSelect.disabled = false;
                var activeAreaLabel = areaSelect.options[areaSelect.selectedIndex]
                    ? areaSelect.options[areaSelect.selectedIndex].text
                    : "";
                if (normalizeAreaId(assignModalState.selectedAreaId)) {
                    setAreaHint(areaHint, "Area terpilih: " + activeAreaLabel + " (" + assignModalState.selectedAreaId + ")", "active");
                } else {
                    setAreaHint(areaHint, "Pilih area penugasan.", "");
                }
            }

            function syncReportAreaDropdown() {
                var areaSelect = document.getElementById("assignReportAreaSelect");
                var areaHint = document.getElementById("assignReportAreaHint");
                if (!areaSelect) {
                    return;
                }

                if (areaLookupState.loading && !areaLookupState.loaded) {
                    areaSelect.innerHTML = "<option value=\"\">Memuat area...</option>";
                    areaSelect.disabled = true;
                    setAreaHint(areaHint, "Memuat daftar area...", "");
                    return;
                }

                if (!areaLookupState.rows.length) {
                    areaSelect.innerHTML = "<option value=\"\">Area tidak tersedia</option>";
                    areaSelect.disabled = true;
                    setAreaHint(areaHint, "Area tidak tersedia. Coba pilih regional spesifik, atau refresh halaman.", "error");
                    return;
                }

                renderAreaSelectOptions(areaSelect, reportModalState.selectedAreaId);
                if (!normalizeAreaId(reportModalState.selectedAreaId) && areaLookupState.rows.length) {
                    reportModalState.selectedAreaId = normalizeAreaId(areaLookupState.rows[0].AreaID);
                    renderAreaSelectOptions(areaSelect, reportModalState.selectedAreaId);
                }
                areaSelect.disabled = false;
                var activeAreaLabel = areaSelect.options[areaSelect.selectedIndex]
                    ? areaSelect.options[areaSelect.selectedIndex].text
                    : "";
                if (normalizeAreaId(reportModalState.selectedAreaId)) {
                    setAreaHint(areaHint, "Area terpilih: " + activeAreaLabel + " (" + reportModalState.selectedAreaId + ")", "active");
                } else {
                    setAreaHint(areaHint, "Pilih area penugasan.", "");
                }
            }

            function isNumericStatus(status) {
                return /^\d+$/.test((status || "").toString().trim());
            }

            function normalizeStatusCode(status) {
                var normalizedStatus = (status || "").toString().trim().toUpperCase();
                if (normalizedStatus === "I") {
                    return "IZ";
                }
                if (normalizedStatus === "C") {
                    return "CT";
                }
                if (normalizedStatus === "OFF") {
                    return "OF";
                }
                return normalizedStatus;
            }

            function isValidItId(value) {
                var normalized = (value || "").toString().trim().toUpperCase();
                return /^IT\d+$/.test(normalized);
            }

            function requireValidItId(cell, showAlert) {
                var itId = cell ? ((cell.getAttribute("data-it-id") || "").trim()) : "";
                if (isValidItId(itId)) {
                    return itId.toUpperCase();
                }

                // Fallback: server will resolve UserID -> conf_mst_user.ITID on save.
                // Only block when attribute is present but clearly invalid (not empty).
                if (itId && !isValidItId(itId)) {
                    if (showAlert !== false) {
                        window.alert("ITID not exist");
                    }
                    return "";
                }

                return (cell ? ((cell.getAttribute("data-tech-id") || "").trim()) : "") ? "__pending__" : "";
            }

            function isStatusOnlyStatus(status) {
                var normalizedStatus = normalizeStatusCode(status);
                return normalizedStatus === "CT" || normalizedStatus === "IZ";
            }

            function isAssignableCell(cell) {
                if (!cell) {
                    return false;
                }
                if (getIsTechnicianUser()) {
                    return false;
                }

                var status = normalizeStatusCode(cell.getAttribute("data-status") || "");
                var cellDate = parseIsoDate(cell.getAttribute("data-date"));
                if (!cellDate) {
                    return false;
                }

                var isFutureOrToday = cellDate.getTime() >= getTodayDateOnly().getTime();
                if (!isFutureOrToday) {
                    return false;
                }

                // AV/AD/OF can open assign modal; ITID is validated on open/submit.
                return status === "AV" || status === "AD" || status === "OF";
            }

            function isStatusOnlyEditableCell(cell) {
                if (!cell) {
                    return false;
                }
                if (getIsTechnicianUser()) {
                    return false;
                }

                var status = normalizeStatusCode(cell.getAttribute("data-status") || "");
                var cellDate = parseIsoDate(cell.getAttribute("data-date"));
                if (!cellDate) {
                    return false;
                }
                var isPastDate = cellDate.getTime() < getTodayDateOnly().getTime();

                if (isStatusOnlyStatus(status)) {
                    return !isPastDate;
                }

                return false;
            }

            function isCompletedReportCell(cell) {
                if (!cell) {
                    return false;
                }

                var status = (cell.getAttribute("data-status") || "").toUpperCase();
                return isNumericStatus(status);
            }

            function setReportFeedback(text, isError, isSuccess) {
                var feedback = document.getElementById("assignReportFeedback");
                if (!feedback) {
                    return;
                }
                feedback.classList.remove("error");
                feedback.classList.remove("success");
                feedback.classList.remove("is-visible");
                feedback.textContent = text || "";
                if (isError) {
                    feedback.classList.add("error");
                }
                if (isSuccess) {
                    feedback.classList.add("success");
                }
                if (text) {
                    feedback.classList.add("is-visible");
                }
            }

            function hasCompletedStatusInReportRows() {
                var rows = reportModalState.rows || [];
                for (var i = 0; i < rows.length; i++) {
                    var row = rows[i] || {};
                    var statusCode = (row.StatusCode || "").toUpperCase();
                    var statusText = (row.StatusText || "").toLowerCase();
                    if (statusCode === "CL" || statusText === "selesai") {
                        return true;
                    }
                }
                return false;
            }

            function formatScheduleAssignDate(dateText) {
                var text = (dateText || "").toString().trim();
                if (!text) {
                    return "-";
                }
                if (parseIsoDate(text)) {
                    return formatDisplayDate(text);
                }
                return text;
            }

            function getScheduleRowCustomerName(row) {
                row = row || {};
                var name = (row.CustomerName || row.FullName || "").toString().trim();
                if (!name) {
                    var customer = (row.Customer || "").toString().trim();
                    var custId = (row.CustID || "").toString().trim();
                    if (customer && customer !== custId) {
                        if (customer.indexOf(" - ") >= 0) {
                            name = customer.split(" - ").slice(1).join(" - ").trim();
                        } else if (!custId || customer.toUpperCase() !== custId.toUpperCase()) {
                            name = customer;
                        }
                    }
                }
                return name || "-";
            }

            function renderScheduleReportTableRows() {
                var reportTableBody = document.getElementById("assignReportTableBody");
                if (!reportTableBody) {
                    return;
                }

                if (reportModalState.isLoading) {
                    reportTableBody.innerHTML = "<tr><td class=\"assign-report-empty\" colspan=\"10\">"
                        + "<div class=\"assign-closed-loading\" style=\"min-height:140px;\">"
                        + "<span class=\"assign-closed-loading-spinner\" aria-hidden=\"true\"></span>"
                        + "<span>Memuat data Training/Visit...</span>"
                        + "</div></td></tr>";
                    return;
                }

                var rows = reportModalState.rows || [];
                if (!rows.length) {
                    reportTableBody.innerHTML = "<tr><td class=\"assign-report-empty\" colspan=\"10\">Belum ada data schedule pada tanggal ini.</td></tr>";
                    return;
                }

                var html = [];
                for (var i = 0; i < rows.length; i++) {
                    var row = rows[i] || {};
                    var isCompleted = isScheduleRowCompleted(row);
                    var statusText = row.StatusText || row.StatusCode || (isCompleted ? "Selesai" : "Belum Selesai");
                    var badgeClass = isCompleted ? "assign-report-status done" : "assign-report-status pending";
                    var canDelete = row.CanDelete && !getIsTechnicianUser();
                    var canEdit = canDelete;
                    var detailButton = "<button type=\"button\" class=\"assign-report-training-btn\" data-row-index=\"" + i + "\">Info</button>";
                    var installButton = "";
                    var editButton = canEdit
                        ? "<button type=\"button\" class=\"assign-report-edit-btn\" data-row-index=\"" + i + "\">Edit</button>"
                        : "";
                    var deleteButton = canDelete
                        ? "<button type=\"button\" class=\"assign-report-delete-btn\" data-row-index=\"" + i + "\">Delete</button>"
                        : "";
                    var actionButton = "<div class=\"assign-report-action-wrap\">" + detailButton + installButton + editButton + deleteButton + "</div>";
                    var customerName = getScheduleRowCustomerName(row);
                    html.push("<tr>"
                        + "<td class=\"col-action\">" + actionButton + "</td>"
                        + "<td><span class=\"" + badgeClass + "\">" + escapeHtml(statusText) + "</span></td>"
                        + "<td class=\"col-customer\">" + escapeHtml(customerName) + "</td>"
                        + "<td>" + escapeHtml(row.JobID || "-") + "</td>"
                        + "<td>" + escapeHtml(formatDisplayDate(row.SchDate || reportModalState.schDate)) + "</td>"
                        + "<td>" + escapeHtml(formatScheduleAssignDate(row.AssignDate)) + "</td>"
                        + "<td class=\"assign-jo-address-col\">" + escapeHtml(row.Address || "-") + "</td>"
                        + "<td>" + escapeHtml(row.PicName || "-") + "</td>"
                        + "<td>" + escapeHtml(row.TechnicianName || reportModalState.technicianName || "-") + "</td>"
                        + "</tr>");
                }
                reportTableBody.innerHTML = html.join("");
            }

            function openReportRemarkModal(row) {
                openTrainingInfoModal(row);
            }

            function openTrainingInfoModal(row) {
                var backdrop = getReportRemarkBackdrop();
                if (!backdrop) {
                    return;
                }

                var title = document.getElementById("assignReportRemarkTitle");
                var subtitle = document.getElementById("assignReportRemarkSubtitle");
                var content = document.getElementById("assignReportRemarkContent");
                var jobId = ((row && row.JobID) || "-").toString();
                var customer = getScheduleRowCustomerName(row);
                var schDate = formatDisplayDate((row && row.SchDate) || "");
                var assignDate = formatScheduleAssignDate((row && row.AssignDate) || "");
                var address = ((row && row.Address) || "-").toString();
                var picName = ((row && row.PicName) || "-").toString();
                var jobTypeText = ((row && row.JobType) || "").toString().toLowerCase();
                var isVisit = jobTypeText.indexOf("visit") >= 0;
                var remarkText = ((row && row.Remark) || "").toString().trim();
                if (!remarkText) {
                    remarkText = "Tidak ada remark.";
                }

                if (title) {
                    title.textContent = isVisit ? "Detail Visit" : "Detail Training";
                }
                if (subtitle) {
                    subtitle.textContent = (isVisit ? "Visit ID: " : "Training ID: ") + jobId;
                }
                if (content) {
                    content.innerHTML = ""
                        + "<div><strong>Customer:</strong> " + escapeHtml(customer) + "</div>"
                        + "<div><strong>Tanggal Jadwal:</strong> " + escapeHtml(schDate || "-") + "</div>"
                        + "<div><strong>Tanggal Assign:</strong> " + escapeHtml(assignDate || "-") + "</div>"
                        + "<div><strong>Alamat:</strong> " + escapeHtml(address) + "</div>"
                        + "<div><strong>PIC Customer:</strong> " + escapeHtml(picName) + "</div>"
                        + "<div style=\"margin-top:8px;\"><strong>Catatan:</strong><br>" + escapeHtml(remarkText) + "</div>";
                }
                backdrop.classList.add("open");
            }

            function closeReportRemarkModal() {
                var backdrop = getReportRemarkBackdrop();
                if (backdrop) {
                    backdrop.classList.remove("open");
                }
            }

            function renderReportSummary() {
                var tech = document.getElementById("assignReportTechName");
                var date = document.getElementById("assignReportDate");
                var total = document.getElementById("assignReportTotalUnit");
                var totalPending = document.getElementById("assignReportTotalUnitBelumSelesai");
                var rows = reportModalState.rows || [];

                if (tech) {
                    tech.textContent = reportModalState.technicianName || "-";
                }
                if (date) {
                    date.textContent = formatDisplayDate(reportModalState.schDate || "");
                }

                if (reportModalState.isLoading) {
                    if (total) {
                        total.textContent = "...";
                    }
                    if (totalPending) {
                        totalPending.textContent = "...";
                    }
                    return;
                }

                var computedDone = 0;
                for (var i = 0; i < rows.length; i++) {
                    var row = rows[i] || {};
                    var statusCode = (row.StatusCode || "").toUpperCase();
                    var statusText = (row.StatusText || "").toLowerCase();
                    if (statusCode === "CL" || statusCode === "CLOSE" || statusCode === "CLOSED" || statusText === "selesai" || statusText === "close") {
                        computedDone++;
                    }
                }
                var totalDone = reportModalState.totalUnitSelesai > 0 || rows.length === 0
                    ? reportModalState.totalUnitSelesai
                    : computedDone;
                var totalNotDone = reportModalState.totalUnitBelumSelesai > 0 || rows.length === 0
                    ? reportModalState.totalUnitBelumSelesai
                    : Math.max(0, rows.length - totalDone);
                if (total) {
                    total.textContent = Math.max(0, totalDone).toString();
                }
                if (totalPending) {
                    totalPending.textContent = Math.max(0, totalNotDone).toString();
                }
            }

            function getCurrentReportOrder() {
                return reportModalState.selectedOrder;
            }

            function syncReportAssignInputHint(assignedUnit) {
                var inputUnit = document.getElementById("assignReportInputUnit");
                var inputLabel = document.getElementById("assignReportInputUnitLabel");
                var inputHint = document.getElementById("assignReportInputUnitHint");
                var safeAssigned = Math.max(0, toInt(assignedUnit, 0));
                if (inputLabel) {
                    inputLabel.textContent = "Assign IT Support";
                }
                if (inputUnit) {
                    inputUnit.value = "1";
                    inputUnit.placeholder = "1";
                    inputUnit.max = "1";
                }
                if (inputHint) {
                    inputHint.textContent = "Sudah Assign IT Support : " + safeAssigned.toString();
                }
            }

            function setActiveReportAssignButtons() {
                var jobTypeButtons = document.querySelectorAll("#assignReportJobTypeWrap .assign-jo-type-btn");
                for (var i = 0; i < jobTypeButtons.length; i++) {
                    var jobTypeButton = jobTypeButtons[i];
                    jobTypeButton.classList.toggle("active", normalizeJoTypeForButton(jobTypeButton.getAttribute("data-jo-type")) === reportModalState.joType);
                }

                var hasCompletedStatus = hasCompletedStatusInReportRows();
                if (hasCompletedStatus && reportModalState.targetStatus !== "AV") {
                    reportModalState.targetStatus = "AV";
                }

                var statusButtons = document.querySelectorAll("#assignReportStatusWrap .assign-status-btn");
                for (var s = 0; s < statusButtons.length; s++) {
                    var statusButton = statusButtons[s];
                    var statusValue = (statusButton.getAttribute("data-status-value") || "AV").toUpperCase();
                    var isNonAvailableStatus = statusValue === "OF" || statusValue === "CT" || statusValue === "IZ";
                    statusButton.disabled = hasCompletedStatus && isNonAvailableStatus;
                    statusButton.classList.toggle("active", statusButton.getAttribute("data-status-value") === reportModalState.targetStatus);
                }

                var groupButtons = document.querySelectorAll("#assignReportDeviceGroupWrap .assign-jo-type-btn");
                var isMaintenance = normalizeJoTypeForApi(reportModalState.joType) === "maintenance";
                for (var j = 0; j < groupButtons.length; j++) {
                    var groupButton = groupButtons[j];
                    var group = normalizeDeviceGroup(groupButton.getAttribute("data-device-group"));
                    var disabled = isMaintenance && group === "ACS";
                    groupButton.disabled = disabled;
                    groupButton.classList.toggle("active", group === reportModalState.deviceGroupId);
                }
            }

            function resetReportAssignFormFields() {
                reportModalState.selectedOrder = null;
                reportModalState.editingRowIndex = -1;
                reportModalState.joType = "new_install";
                reportModalState.deviceGroupId = "GPS";
                reportModalState.targetStatus = "AV";
                reportModalState.selectedAreaId = "";
                clearDashboardNote("assignReportTransferNoteInput");
                if (areaLookupState.rows.length) {
                    reportModalState.selectedAreaId = normalizeAreaId(areaLookupState.rows[0].AreaID);
                }
            }

            function openReportAssignCreator() {
                reportModalState.pendingDeleteRowIndex = -1;
                resetReportAssignFormFields();
                reportModalState.showAssignForm = true;
                renderReportDeletePanel();
                renderReportAssignForm();
                setReportFeedback("", false, false);
            }

            function cancelReportAssignForm() {
                reportModalState.showAssignForm = false;
                resetReportAssignFormFields();
                renderReportAssignForm();
                setReportFeedback("", false, false);
            }

            function renderReportAssignForm() {
                var assignCard = document.getElementById("assignReportAssignCard");
                var intro = document.getElementById("assignReportAssignIntro");
                var formWrap = document.getElementById("assignReportAssignForm");
                var isTechnicianUser = getIsTechnicianUser();
                var isPastDate = isPastScheduleDate(reportModalState.schDate || "");
                if (assignCard) {
                    assignCard.style.display = (isTechnicianUser || isPastDate) ? "none" : "";
                }
                if (isTechnicianUser || isPastDate) {
                    return;
                }

                if (reportModalState.pendingDeleteRowIndex >= 0) {
                    if (assignCard) {
                        assignCard.style.display = "none";
                    }
                    renderReportDeletePanel();
                    return;
                }

                var showForm = reportModalState.showAssignForm || reportModalState.editingRowIndex >= 0;
                if (intro) {
                    intro.hidden = showForm;
                    intro.style.display = showForm ? "none" : "";
                }
                if (formWrap) {
                    formWrap.hidden = !showForm;
                    formWrap.style.display = showForm ? "" : "none";
                }
                if (!showForm) {
                    return;
                }

                var selected = getCurrentReportOrder();
                var pickedJo = document.getElementById("assignReportPickedJoText");
                var customer = document.getElementById("assignReportInfoCustomer");
                var remainGps = document.getElementById("assignReportRemainingGps");
                var remainAcs = document.getElementById("assignReportRemainingAcs");
                var input = document.getElementById("assignReportInputUnit");
                var areaSelect = document.getElementById("assignReportAreaSelect");
                var assignBtn = document.getElementById("assignReportAssignBtn");

                if (normalizeJoTypeForApi(reportModalState.joType) === "maintenance" && reportModalState.deviceGroupId !== "GPS") {
                    reportModalState.deviceGroupId = "GPS";
                }

                setActiveReportAssignButtons();

                if (pickedJo) {
                    var transferLabel = selected ? getJobOrderTransferLabel(selected, reportModalState.technicianId) : "";
                    pickedJo.textContent = selected
                        ? (selected.JobID + " - " + getCustomerDisplayText(selected) + " (" + (selected.BranchName || "-") + ")" + (transferLabel ? " [" + transferLabel + "]" : ""))
                        : "Belum ada Job Order dipilih";
                }
                if (customer) {
                    customer.textContent = selected ? getCustomerDisplayText(selected) : "-";
                }

                var remainingGps = selected ? toInt(selected.RemainingUnitGps, 0) : 0;
                var remainingAcs = selected ? toInt(selected.RemainingUnitAcs, 0) : 0;
                var customerGps = selected ? getCustomerGpsCount(selected) : 0;
                var customerAcs = selected ? getCustomerAcsCount(selected) : 0;
                var assignedItSupport = selected ? toInt(selected.TotalAssign, 0) : 0;
                if (remainGps) {
                    remainGps.textContent = customerGps.toString();
                }
                if (remainAcs) {
                    remainAcs.textContent = customerAcs.toString();
                }

                var selectedRemaining = reportModalState.deviceGroupId === "ACS" ? remainingAcs : remainingGps;
                syncReportAssignInputHint(assignedItSupport);
                syncReportAreaDropdown();

                if (input) {
                    input.value = "1";
                }

                var disableGps = remainingGps <= 0;
                var disableAcs = remainingAcs <= 0 || normalizeJoTypeForApi(reportModalState.joType) === "maintenance";
                var groupButtons = document.querySelectorAll("#assignReportDeviceGroupWrap .assign-jo-type-btn");
                for (var idx = 0; idx < groupButtons.length; idx++) {
                    var btn = groupButtons[idx];
                    var grp = normalizeDeviceGroup(btn.getAttribute("data-device-group"));
                    if (grp === "GPS") {
                        btn.disabled = disableGps;
                    } else if (grp === "ACS") {
                        btn.disabled = disableAcs;
                    }
                }

                if (reportModalState.deviceGroupId === "GPS" && disableGps && !disableAcs) {
                    reportModalState.deviceGroupId = "ACS";
                    selectedRemaining = remainingAcs;
                    syncReportAssignInputHint(assignedItSupport);
                } else if (reportModalState.deviceGroupId === "ACS" && disableAcs && !disableGps) {
                    reportModalState.deviceGroupId = "GPS";
                    selectedRemaining = remainingGps;
                    syncReportAssignInputHint(assignedItSupport);
                }
                setActiveReportAssignButtons();

                if (assignBtn) {
                    var isEditing = reportModalState.editingRowIndex >= 0;
                    var canAssignNew = selected && selectedRemaining > 0;
                    var bothZero = !canAssignNew && !isEditing;
                    assignBtn.disabled = bothZero || reportModalState.isSaving;
                    var isAssignLoading = reportModalState.isSaving && reportModalState.submitAction === "assign";
                    assignBtn.classList.toggle("is-loading", isAssignLoading);
                    assignBtn.textContent = isAssignLoading
                        ? "Saving..."
                        : (isEditing ? "Simpan Perubahan" : "Assign");
                }
                if (areaSelect && normalizeAreaId(areaSelect.value) !== normalizeAreaId(reportModalState.selectedAreaId)) {
                    areaSelect.value = normalizeAreaId(reportModalState.selectedAreaId);
                }
                syncTransferNoteField("assignReportTransferNoteWrap", "assignReportTransferNoteInput", selected, reportModalState.technicianId);
            }

            function validateReportAssignInput(showFeedback) {
                var selectedStatus = (reportModalState.targetStatus || "AV").toUpperCase();
                if (selectedStatus !== "AV") {
                    return true;
                }

                var selected = getCurrentReportOrder();
                if (!selected) {
                    if (showFeedback) {
                        setReportFeedback("Job Order wajib dipilih untuk status Available.", true, false);
                    }
                    return false;
                }

                var remaining = reportModalState.deviceGroupId === "ACS"
                    ? toInt(selected.RemainingUnitAcs, 0)
                    : toInt(selected.RemainingUnitGps, 0);
                var qty = 1;
                var groupText = reportModalState.deviceGroupId === "ACS" ? "ACS" : "GPS";
                if (reportModalState.editingRowIndex < 0 && remaining <= 0 && !isJobOrderTransfer(selected, reportModalState.technicianId)) {
                    if (showFeedback) {
                        var assignedCount = toInt(selected.TotalAssign, 0);
                        if (assignedCount > 0) {
                            setReportFeedback("Job Order ini sudah di-assign ke IT Support. Cari Job ID tepat di picker untuk pindah.", true, false);
                        } else {
                            setReportFeedback("Job Order tidak bisa di-assign saat ini.", true, false);
                        }
                    }
                    return false;
                }
                if (requiresTransferNoteForOrder(selected, reportModalState.technicianId)) {
                    var transferNote = getDashboardNoteValue("assignReportTransferNoteInput");
                    if (!transferNote) {
                        if (showFeedback) {
                            setReportFeedback("Catatan pindah IT Support wajib diisi.", true, false);
                            var transferInput = document.getElementById("assignReportTransferNoteInput");
                            if (transferInput) {
                                transferInput.focus();
                            }
                        }
                        return false;
                    }
                    if (showFeedback) {
                        setReportFeedback(getJobOrderTransferLabel(selected, reportModalState.technicianId), false, false);
                    }
                }
                return true;
            }

            function renderCompletedReportModal(response) {
                reportModalState.isLoading = false;
                reportModalState.rows = response && response.Rows ? response.Rows : [];
                reportModalState.totalUnitSelesai = toInt(response && response.TotalUnitSelesai, 0);
                reportModalState.totalUnitBelumSelesai = toInt(response && response.TotalUnitBelumSelesai, 0);
                if (response && response.TechnicianName) {
                    reportModalState.technicianName = response.TechnicianName;
                }
                if (response && response.SchDate) {
                    reportModalState.schDate = response.SchDate;
                }
                renderReportSummary();
                renderScheduleReportTableRows();
                renderReportDeletePanel();
                renderReportAssignForm();
            }

            function loadCompletedReportModalData(showFeedbackOnError) {
                var requestToken = (reportModalState.requestToken || 0) + 1;
                reportModalState.requestToken = requestToken;
                reportModalState.isLoading = true;
                renderReportSummary();
                renderScheduleReportTableRows();

                callAssignPageMethod(
                    "LoadScheduleReport",
                    {
                        technicianId: reportModalState.technicianId || "",
                        schDate: reportModalState.schDate || "",
                        technicianName: reportModalState.technicianName || ""
                    },
                    function (result) {
                        if (requestToken !== reportModalState.requestToken) {
                            return;
                        }

                        var isSuccess = (result && result.Result ? result.Result : "").toUpperCase() === "SUCCESS";
                        if (!isSuccess) {
                            reportModalState.isLoading = false;
                            var failMessage = (result && result.Message) || "Gagal memuat laporan schedule.";
                            if ((failMessage || "").toLowerCase().indexOf("itid") >= 0) {
                                window.alert("ITID not exist");
                            }
                            if (showFeedbackOnError) {
                                setReportFeedback(failMessage, true, false);
                            }
                            reportModalState.rows = [];
                            renderReportSummary();
                            renderScheduleReportTableRows();
                            return;
                        }

                        setReportFeedback("", false, false);
                        renderCompletedReportModal(result || {});
                    },
                    function (errorMessage) {
                        if (requestToken !== reportModalState.requestToken) {
                            return;
                        }

                        reportModalState.isLoading = false;
                        reportModalState.rows = [];
                        renderReportSummary();
                        renderScheduleReportTableRows();
                        if (showFeedbackOnError) {
                            setReportFeedback("Gagal memuat laporan schedule. " + (errorMessage || ""), true, false);
                        }
                    });
            }

            function openCompletedReportModal(cell) {
                if (!isCompletedReportCell(cell)) {
                    return;
                }
                if (!requireValidItId(cell, true)) {
                    return;
                }

                reportModalState.activeCell = cell;
                reportModalState.technicianId = (cell.getAttribute("data-tech-id") || "").trim();
                reportModalState.technicianName = (cell.getAttribute("data-tech-name") || "").trim();
                reportModalState.schDate = (cell.getAttribute("data-date") || "").trim();
                reportModalState.rows = [];
                reportModalState.selectedOrder = null;
                reportModalState.editingRowIndex = -1;
                reportModalState.joType = "new_install";
                reportModalState.deviceGroupId = "GPS";
                reportModalState.targetStatus = "AV";
                reportModalState.supAreaId = (cell.getAttribute("data-sup-area") || "").trim();
                reportModalState.selectedAreaId = "";
                reportModalState.isSaving = false;
                reportModalState.isLoading = true;
                reportModalState.submitAction = "assign";
                reportModalState.totalUnitSelesai = 0;
                reportModalState.totalUnitBelumSelesai = 0;
                reportModalState.showAssignForm = false;
                reportModalState.pendingDeleteRowIndex = -1;

                var subtitle = document.getElementById("assignReportSubtitle");
                if (subtitle) {
                    subtitle.textContent = "Daftar Training/Visit pada IT Support terpilih";
                }

                var reportTitle = document.getElementById("assignReportTitle");
                if (reportTitle) {
                    reportTitle.textContent = "Jadwal Training / Visit";
                }

                renderReportSummary();
                renderScheduleReportTableRows();
                renderReportDeletePanel();
                renderReportAssignForm();
                setReportFeedback("Memuat data laporan...", false, false);

                var backdrop = getCompletedReportBackdrop();
                if (backdrop) {
                    backdrop.classList.add("open");
                }

                fetchAreaOptions(reportModalState.supAreaId, function () {
                    if (!normalizeAreaId(reportModalState.selectedAreaId) && areaLookupState.rows.length) {
                        reportModalState.selectedAreaId = normalizeAreaId(areaLookupState.rows[0].AreaID);
                    }
                    renderReportAssignForm();
                });
                loadCompletedReportModalData(true);
            }

            function closeCompletedReportModal() {
                var backdrop = getCompletedReportBackdrop();
                if (backdrop) {
                    backdrop.classList.remove("open");
                }
                closeReportRemarkModal();
                reportModalState.isSaving = false;
                reportModalState.isLoading = false;
                reportModalState.requestToken = (reportModalState.requestToken || 0) + 1;
                setReportFeedback("", false, false);
            }

            function setReportAssignSaving(isLoading) {
                reportModalState.isSaving = !!isLoading;
                renderReportAssignForm();
            }

            function submitAssignFromReportModal(actionSource) {
                if (reportModalState.isSaving) {
                    return;
                }
                if (!requireValidItId(reportModalState.activeCell, true)) {
                    return;
                }
                reportModalState.submitAction = actionSource === "administration" ? "administration" : "assign";
                if (getIsTechnicianUser()) {
                    setReportFeedback("User IT Support hanya dapat melihat detail informasi.", true, false);
                    return;
                }
                if (isPastScheduleDate(reportModalState.schDate || "")) {
                    setReportFeedback("Assign Job hanya tersedia untuk tanggal hari ini atau setelahnya.", true, false);
                    return;
                }

                var selectedStatus = reportModalState.submitAction === "administration"
                    ? "AD"
                    : (reportModalState.targetStatus || "AV").toUpperCase();
                var isAvailableStatus = selectedStatus === "AV";
                var selectedDeviceGroup = normalizeJoTypeForApi(reportModalState.joType) === "maintenance"
                    ? "GPS"
                    : normalizeDeviceGroup(reportModalState.deviceGroupId);
                var selectedOrder = getCurrentReportOrder();
                if (isAvailableStatus && !selectedOrder) {
                    setReportFeedback("Belum ada JO yang dipilih.", true, false);
                    return;
                }

                if (!reportModalState.technicianId || !reportModalState.schDate) {
                    setReportFeedback("IT Support atau tanggal schedule tidak valid.", true, false);
                    return;
                }

                var qty = 0;
                var selectedAreaId = "";
                if (isAvailableStatus) {
                    if (!validateReportAssignInput(true)) {
                        return;
                    }
                    qty = 1;
                    selectedAreaId = selectedOrder ? normalizeAreaId(selectedOrder.DefaultAreaId || "") : "";
                }

                var editingRowIndex = reportModalState.editingRowIndex;
                var editingRow = editingRowIndex >= 0 ? ((reportModalState.rows || [])[editingRowIndex] || null) : null;

                function proceedReportAssignSave(assignRemark) {
                function finishAssignSave(result) {
                    setReportFeedback((result && result.Message) || "Assignment berhasil disimpan.", false, true);
                    reportModalState.showAssignForm = false;
                    reportModalState.editingRowIndex = -1;
                    var sourceTechId = (result && result.PreviousTechnicianId) || getOrderAssignedTechnicianId(selectedOrder);
                    var previousSchDate = (result && result.PreviousSchDate)
                        || getOrderAssignedSchDate(selectedOrder)
                        || (editingRow && editingRow.SchDate)
                        || "";
                    var techIds = [reportModalState.technicianId];
                    if (sourceTechId && normalizeTechId(sourceTechId) !== normalizeTechId(reportModalState.technicianId)) {
                        techIds.push(sourceTechId);
                    }
                    reportModalState.selectedOrder = null;
                    clearDashboardNote("assignReportTransferNoteInput");
                    loadCompletedReportModalData(false);
                    refreshAfterAssignChange({
                        scheduleDate: (result && result.SchDate) || reportModalState.schDate,
                        technicianIds: techIds,
                        previousSchDate: previousSchDate,
                        previousTechnicianId: sourceTechId,
                        primaryCell: reportModalState.activeCell
                    }, function () {
                        setReportAssignSaving(false);
                    });
                }

                function runSaveAssign() {
                    callAssignPageMethod(
                        "SaveAssignJob",
                        {
                            assignId: "",
                            jobId: selectedOrder ? (selectedOrder.JobID || "") : "",
                            custId: selectedOrder ? (selectedOrder.Customer || "") : "",
                            technicianId: reportModalState.technicianId,
                            schDate: reportModalState.schDate,
                            qtyAssign: qty,
                            deviceGroupId: selectedDeviceGroup,
                            areaId: selectedAreaId,
                            targetStatus: selectedStatus,
                            insDeviceTypeId: getInsDeviceTypeId(reportModalState.joType, selectedOrder),
                            assignRemark: assignRemark
                        },
                        function (result) {
                            var success = (result && result.Result ? result.Result : "").toUpperCase() === "SUCCESS";
                            if (!success) {
                                setReportAssignSaving(false);
                                var failedMessage = (result && result.Message) || "Gagal menyimpan assignment.";
                                if ((failedMessage || "").toLowerCase().indexOf("catatan pindah") >= 0 && selectedOrder) {
                                    selectedOrder._requiresTransferNote = true;
                                    reportModalState.selectedOrder = selectedOrder;
                                    renderReportAssignForm();
                                }
                                setReportFeedback(failedMessage, true, false);
                                return;
                            }
                            finishAssignSave(result);
                        },
                        function (errorMessage) {
                            setReportAssignSaving(false);
                            setReportFeedback("Gagal menyimpan assignment. " + (errorMessage || ""), true, false);
                        });
                }

                if (editingRow && editingRow.AssignID) {
                    callAssignPageMethod(
                        "DeleteScheduleAssign",
                        {
                            assignId: editingRow.AssignID || "",
                            seq: toInt(editingRow.Seq, -1)
                        },
                        function (deleteResult) {
                            var deleteSuccess = (deleteResult && deleteResult.Result ? deleteResult.Result : "").toUpperCase() === "SUCCESS";
                            if (!deleteSuccess) {
                                setReportAssignSaving(false);
                                setReportFeedback((deleteResult && deleteResult.Message) || "Gagal menghapus assign sebelum edit.", true, false);
                                return;
                            }
                            runSaveAssign();
                        },
                        function (errorMessage) {
                            setReportAssignSaving(false);
                            setReportFeedback("Gagal menghapus assign sebelum edit. " + (errorMessage || ""), true, false);
                        });
                    return;
                }

                setReportAssignSaving(true);
                setReportFeedback("Menyimpan assignment...", false, false);
                runSaveAssign();
                }

                if (!isAvailableStatus || !selectedOrder) {
                    proceedReportAssignSave("");
                    return;
                }

                enrichSelectedOrderAssignContext(selectedOrder, "report", function (enriched) {
                    reportModalState.selectedOrder = enriched;
                    renderReportAssignForm();
                    var assignRemark = "";
                    if (requiresTransferNoteForOrder(enriched, reportModalState.technicianId)) {
                        assignRemark = getDashboardNoteValue("assignReportTransferNoteInput");
                        if (!assignRemark) {
                            setReportFeedback("Catatan pindah IT Support wajib diisi.", true, false);
                            var transferInput = document.getElementById("assignReportTransferNoteInput");
                            if (transferInput) {
                                transferInput.focus();
                            }
                            return;
                        }
                    }
                    proceedReportAssignSave(assignRemark);
                });
            }

            function editAssignFromReportRow(rowIndex) {
                if (getIsTechnicianUser()) {
                    setReportFeedback("User IT Support tidak memiliki akses untuk mengedit assignment.", true, false);
                    return;
                }

                var rows = reportModalState.rows || [];
                if (rowIndex < 0 || rowIndex >= rows.length) {
                    return;
                }

                var row = rows[rowIndex] || {};
                if (!row.CanDelete) {
                    return;
                }

                reportModalState.pendingDeleteRowIndex = -1;
                reportModalState.showAssignForm = true;
                reportModalState.editingRowIndex = rowIndex;
                reportModalState.targetStatus = "AV";
                reportModalState.selectedOrder = {
                    JobID: row.JobID || "",
                    Customer: row.CustID || "",
                    CustomerName: getScheduleRowCustomerName(row),
                    BranchName: "",
                    RemainingUnitGps: 1,
                    RemainingUnitAcs: 0,
                    TotalAssignGps: 0,
                    TotalAssignAcs: 0,
                    DefaultAreaId: row.AreaID || ""
                };
                reportModalState.selectedAreaId = normalizeAreaId(row.AreaID || "");
                var jobTypeText = ((row.JobType || "") + "").toLowerCase();
                reportModalState.joType = jobTypeText.indexOf("visit") >= 0 ? "maintenance" : "new_install";
                renderReportDeletePanel();
                renderReportAssignForm();
                setReportFeedback("Mode edit: ubah Area lalu klik Simpan Perubahan.", false, false);
            }

            function deleteAssignFromReportRow(rowIndex) {
                if (getIsTechnicianUser()) {
                    setReportFeedback("User IT Support tidak memiliki akses untuk menghapus assignment.", true, false);
                    return;
                }

                var rows = reportModalState.rows || [];
                if (rowIndex < 0 || rowIndex >= rows.length) {
                    return;
                }

                var row = rows[rowIndex] || {};
                if (!row.CanDelete) {
                    return;
                }

                if (!row.AssignID) {
                    setReportFeedback("AssignID tidak ditemukan untuk data yang dipilih.", true, false);
                    return;
                }
                if (typeof row.Seq === "undefined" || row.Seq === null || toInt(row.Seq, -1) < 0) {
                    setReportFeedback("Seq tidak ditemukan untuk data yang dipilih.", true, false);
                    return;
                }

                openReportDeletePanel(rowIndex);
            }

            function openDayTotalJoModal(trigger) {
                var schDate = trigger ? (trigger.getAttribute("data-date") || "").trim() : "";
                var totalJo = toInt(trigger ? trigger.getAttribute("data-total-jo") : 0, 0);
                var title = document.getElementById("assignDayTotalJoTitle");
                var count = document.getElementById("assignDayTotalJoCount");
                var unitCount = document.getElementById("assignDayTotalJoUnitCount");
                var content = document.getElementById("assignDayTotalJoContent");
                var requestToken = dayTotalJoRequestToken + 1;
                dayTotalJoRequestToken = requestToken;

                if (title) {
                    title.textContent = formatDisplayDate(schDate);
                }
                if (count) {
                    count.textContent = totalJo.toString();
                }
                if (unitCount) {
                    unitCount.textContent = "0";
                }
                if (content) {
                    content.innerHTML = "<div class=\"assign-closed-loading\"><span class=\"assign-closed-loading-spinner\" aria-hidden=\"true\"></span><span>Memuat daftar JO per tanggal...</span></div>";
                }

                if (typeof $ !== "undefined" && $.fn && $.fn.modal) {
                    $("#assignDayTotalJoModal").modal("show");
                }

                if (!schDate) {
                    if (content && requestToken === dayTotalJoRequestToken) {
                        content.innerHTML = "<div class=\"assign-closed-empty\"><div class=\"assign-closed-empty-icon\" aria-hidden=\"true\">&#128229;</div><p>Tanggal schedule tidak valid.</p></div>";
                    }
                    return;
                }

                var request = new XMLHttpRequest();
                request.open(
                    "GET",
                    getAssignPageUrl()
                        + "?action=load_day_total_jo"
                        + "&scheduleDate=" + encodeURIComponent(schDate),
                    true);
                request.setRequestHeader("Accept", "application/json");
                request.onreadystatechange = function () {
                    if (request.readyState !== 4) {
                        return;
                    }
                    if (requestToken !== dayTotalJoRequestToken) {
                        return;
                    }

                    try {
                        var text = String(request.responseText || "").replace(/^\uFEFF/, "").trim();
                        if (!text) {
                            throw new Error("Response kosong dari server.");
                        }
                        if (text.charAt(0) === "<") {
                            throw new Error("Server mengembalikan HTML (session/login/compile).");
                        }

                        var result = JSON.parse(text);
                        if (result && typeof result.d !== "undefined") {
                            result = typeof result.d === "string" ? JSON.parse(result.d) : result.d;
                        }

                        var status = (result && result.Result ? result.Result : "").toUpperCase();
                        var isSuccess = status === "SUCCESS";
                        if (title) {
                            title.textContent = formatDisplayDate((result && result.ScheduleDate) ? result.ScheduleDate : schDate);
                        }
                        if (count) {
                            count.textContent = toInt(result && result.TotalJO, totalJo).toString();
                        }
                        if (unitCount) {
                            unitCount.textContent = toInt(result && result.TotalUnit, 0).toString();
                        }

                        if (!isSuccess) {
                            if (content) {
                                content.innerHTML = "<div class=\"assign-closed-empty\"><div class=\"assign-closed-empty-icon\" aria-hidden=\"true\">&#9888;</div><p>" + escapeHtml((result && result.Message) ? result.Message : "Gagal memuat daftar JO per tanggal.") + "</p></div>";
                            }
                            return;
                        }

                        if (content) {
                            content.innerHTML = (result && result.HtmlContent)
                                ? result.HtmlContent
                                : "<div class=\"assign-closed-empty\"><div class=\"assign-closed-empty-icon\" aria-hidden=\"true\">&#128229;</div><p>Belum ada JO pada tanggal ini.</p></div>";
                        }
                    } catch (ex) {
                        if (content) {
                            var detail = (ex && ex.message) ? ex.message : "Format response tidak valid.";
                            if (request.status && request.status !== 200) {
                                detail = "HTTP " + request.status + " - " + detail;
                            }
                            content.innerHTML = "<div class=\"assign-closed-empty\"><div class=\"assign-closed-empty-icon\" aria-hidden=\"true\">&#9888;</div><p>" + escapeHtml(detail) + "</p></div>";
                        }
                    }
                };
                request.send(null);
            }

            function openTotalJobModal(trigger) {
                var techName = trigger ? (trigger.getAttribute("data-tech-name") || "-") : "-";
                var techId = trigger ? (trigger.getAttribute("data-tech-id") || "") : "";
                var totalJob = toInt(trigger ? trigger.getAttribute("data-total-job") : 0, 0);
                var closedTraining = toInt(trigger ? trigger.getAttribute("data-closed-training") : 0, totalJob);
                var closedVisit = toInt(trigger ? trigger.getAttribute("data-closed-visit") : 0, 0);
                var closeType = trigger ? (trigger.getAttribute("data-close-type") || "new_install") : "new_install";
                var periode = trigger ? (trigger.getAttribute("data-periode") || "") : "";
                var title = document.getElementById("assignTotalJobTitle");
                var count = document.getElementById("assignTotalJobCount");
                var totalUnitCount = document.getElementById("assignTotalUnitCount");
                var content = document.getElementById("assignTotalJobContent");
                var eyebrow = document.getElementById("assignClosedJobEyebrow");
                var requestToken = closedJobRequestToken + 1;
                closedJobRequestToken = requestToken;
                var normalizedCloseType = (closeType || "").toLowerCase() === "maintenance" ? "maintenance" : "new_install";
                var closeTypeLabel = normalizedCloseType === "maintenance"
                    ? "Daftar Job Visit (semua status)"
                    : "Daftar Job Training (semua status)";

                if (title) {
                    title.textContent = techName;
                }
                if (eyebrow) {
                    eyebrow.textContent = closeTypeLabel;
                }
                if (count) {
                    count.textContent = closedTraining.toString();
                }
                if (totalUnitCount) {
                    totalUnitCount.textContent = closedVisit.toString();
                }
                if (content) {
                    content.innerHTML = "<div class=\"assign-closed-loading\"><span class=\"assign-closed-loading-spinner\" aria-hidden=\"true\"></span><span>Memuat daftar Training/Visit...</span></div>";
                }

                if (typeof $ !== "undefined" && $.fn && $.fn.modal) {
                    $("#assignClosedJobModal").modal("show");
                }

                if (!techId) {
                    if (content && requestToken === closedJobRequestToken) {
                        content.innerHTML = "<div class=\"assign-closed-empty\"><div class=\"assign-closed-empty-icon\" aria-hidden=\"true\">&#128229;</div><p>IT Support tidak ditemukan.</p></div>";
                    }
                    return;
                }

                callAssignPageMethod(
                    "GetClosedJobList",
                    {
                        technicianId: techId,
                        technicianName: techName,
                        closeType: normalizedCloseType,
                        periode: periode
                    },
                    function (result) {
                        if (requestToken !== closedJobRequestToken) {
                            return;
                        }

                        var status = (result && result.Result ? result.Result : "").toUpperCase();
                        var isSuccess = status === "SUCCESS";
                        if (title) {
                            title.textContent = (result && result.TechnicianName) ? result.TechnicianName : techName;
                        }
                        if (count) {
                            count.textContent = toInt(result && result.TotalClosedJO, closedTraining).toString();
                        }
                        if (totalUnitCount) {
                            totalUnitCount.textContent = toInt(result && result.TotalClosedUnit, closedVisit).toString();
                        }

                        if (!isSuccess) {
                            if (content) {
                                content.innerHTML = "<div class=\"assign-closed-empty\"><div class=\"assign-closed-empty-icon\" aria-hidden=\"true\">&#9888;</div><p>" + escapeHtml((result && result.Message) ? result.Message : "Gagal memuat daftar pekerjaan selesai.") + "</p></div>";
                            }
                            return;
                        }

                        if (content) {
                            content.innerHTML = (result && result.HtmlContent)
                                ? result.HtmlContent
                                : "<div class=\"assign-closed-empty\"><div class=\"assign-closed-empty-icon\" aria-hidden=\"true\">&#128229;</div><p>Belum ada pekerjaan selesai</p></div>";
                        }
                    },
                    function (errorMessage) {
                        if (requestToken !== closedJobRequestToken) {
                            return;
                        }
                        if (content) {
                            content.innerHTML = "<div class=\"assign-closed-empty\"><div class=\"assign-closed-empty-icon\" aria-hidden=\"true\">&#9888;</div><p>" + escapeHtml(errorMessage || "Gagal memuat daftar pekerjaan selesai.") + "</p></div>";
                        }
                    });
            }

            function closeTotalJobModal() {
                if (typeof $ !== "undefined" && $.fn && $.fn.modal) {
                    $("#assignClosedJobModal").modal("hide");
                }
            }

            function isTotalJobModalOpen() {
                if (typeof $ !== "undefined" && $.fn && $.fn.modal) {
                    return $("#assignClosedJobModal").hasClass("in");
                }

                var modal = getTotalJobBackdrop();
                return !!(modal && modal.style.display === "block");
            }

            function renderStockLoadingSkeleton(message) {
                return "<div class=\"assign-stock-loading\">" + escapeHtml(message || "Memuat stok IT Support...") + "</div>";
            }

            function renderGpsDeviceCards(gpsDevices) {
                var rows = [];
                var tones = ["tone-1", "tone-2", "tone-3", "tone-4"];
                for (var i = 0; i < gpsDevices.length; i++) {
                    var item = gpsDevices[i] || {};
                    var toneClass = tones[i % tones.length];
                    var deviceTypeId = (item.DeviceTypeID || "").toString().trim();
                    var deviceTypeDesc = (item.DeviceTypeDesc || "-").toString();
                    var total = toInt(item.Total, 0);
                    rows.push("<button type=\"button\" class=\"assign-stock-card assign-stock-device-card " + toneClass + "\""
                        + " data-device-type-id=\"" + escapeHtml(deviceTypeId) + "\""
                        + " data-device-type-desc=\"" + escapeHtml(deviceTypeDesc) + "\""
                        + " data-tech-id=\"" + escapeHtml(techStockState.technicianId || "") + "\""
                        + " data-tech-name=\"" + escapeHtml(techStockState.technicianName || "") + "\""
                        + " title=\"Lihat detail " + escapeHtml(deviceTypeDesc) + "\">"
                        + "<div><div class=\"assign-stock-card-name\">" + escapeHtml(deviceTypeDesc) + "</div><div class=\"assign-stock-card-type\">Device Tracking</div></div>"
                        + "<div class=\"assign-stock-card-val\">" + escapeHtml(total.toString()) + "</div>"
                        + "</button>");
                }

                if (!rows.length) {
                    return "<div class=\"assign-stock-empty\">Tidak ada stok tersedia</div>";
                }

                return "<div class=\"assign-stock-grid\">" + rows.join("") + "</div>";
            }

            function renderGsmRows(gsmItems) {
                var rows = [];
                for (var i = 0; i < gsmItems.length; i++) {
                    var item = gsmItems[i] || {};
                    var provider = (item.ProviderID || "-").toString();
                    var qty = toInt(item.Total, 0);
                    rows.push("<div class=\"assign-stock-list-row\"><span>" + escapeHtml(provider) + "</span><span class=\"assign-stock-badge\">" + escapeHtml(qty.toString()) + " pcs</span></div>");
                }
                if (!rows.length) {
                    return "<div class=\"assign-stock-empty\">Tidak ada stok tersedia</div>";
                }
                return "<div class=\"assign-stock-list\">" + rows.join("") + "</div>";
            }

            function renderAccessoryRows(accessories) {
                var rows = [];
                for (var i = 0; i < accessories.length; i++) {
                    var item = accessories[i] || {};
                    var accessoryName = item.DeviceTypeDesc || "-";
                    var accessoryQty = toInt(item.Total, 0);
                    rows.push("<div class=\"assign-stock-list-row\"><span>" + escapeHtml(accessoryName) + "</span><span class=\"assign-stock-badge\">" + escapeHtml(accessoryQty.toString()) + " pcs</span></div>");
                }
                if (!rows.length) {
                    return "<div class=\"assign-stock-empty\">Tidak ada stok tersedia</div>";
                }
                return "<div class=\"assign-stock-list\">" + rows.join("") + "</div>";
            }

            function renderTechStockModalContent(stockData) {
                var gpsDevices = stockData && stockData.GpsDevices ? stockData.GpsDevices : [];
                var gsmItems = stockData && stockData.GsmItems ? stockData.GsmItems : [];
                var accessories = stockData && stockData.Accessories ? stockData.Accessories : [];
                var totalGps = 0;
                var totalGsm = 0;

                for (var i = 0; i < gpsDevices.length; i++) {
                    totalGps += Math.max(0, toInt(gpsDevices[i] && gpsDevices[i].Total, 0));
                }
                for (var j = 0; j < gsmItems.length; j++) {
                    totalGsm += Math.max(0, toInt(gsmItems[j] && gsmItems[j].Total, 0));
                }

                var totalUnit = toInt(stockData && stockData.TotalUnit, (totalGps + totalGsm));
                var hasAnyStock = gpsDevices.length > 0 || gsmItems.length > 0 || accessories.length > 0;
                if (!hasAnyStock) {
                    return "<div class=\"assign-stock-empty\">Tidak ada stok tersedia</div>";
                }

                return "<p class=\"assign-stock-section-title\">Device Tracking &amp; Surveillance</p>"
                    + renderGpsDeviceCards(gpsDevices)
                    + "<p class=\"assign-stock-section-title\">GSM</p>"
                    + renderGsmRows(gsmItems)
                    + "<p class=\"assign-stock-section-title\">Aksesoris</p>"
                    + renderAccessoryRows(accessories)
                    + "<div class=\"assign-stock-total\"><span>Total Semua Stok</span><span>" + escapeHtml(totalUnit.toString()) + " unit</span></div>";
            }

            function getTechDeviceDetailBackdrop() {
                return document.getElementById("assignTechDeviceDetailBackdrop");
            }

            function setTechDeviceDetailFeedback(text, isError) {
                var feedback = document.getElementById("assignTechDeviceDetailFeedback");
                if (!feedback) {
                    return;
                }
                feedback.classList.remove("is-visible");
                feedback.classList.remove("error");
                feedback.textContent = text || "";
                if (text) {
                    feedback.classList.add("is-visible");
                    if (isError) {
                        feedback.classList.add("error");
                    }
                }
            }

            function renderTechDeviceDetailRows() {
                var body = document.getElementById("assignTechDeviceDetailTableBody");
                if (!body) {
                    return;
                }

                var searchText = (techDeviceDetailState.searchText || "").toLowerCase();
                var sourceRows = techDeviceDetailState.rows || [];
                var filtered = [];
                for (var i = 0; i < sourceRows.length; i++) {
                    var row = sourceRows[i] || {};
                    var merged = [
                        row.DeviceID || "",
                        row.NoSN || "",
                        row.DeviceTypeDesc || "",
                        row.StatusText || row.Status || ""
                    ].join(" ").toLowerCase();
                    if (!searchText || merged.indexOf(searchText) >= 0) {
                        filtered.push(row);
                    }
                }

                if (!filtered.length) {
                    body.innerHTML = "<tr><td colspan=\"4\" class=\"assign-jo-empty\">Tidak ada data device.</td></tr>";
                    return;
                }

                var html = [];
                for (var idx = 0; idx < filtered.length; idx++) {
                    var item = filtered[idx] || {};
                    var statusText = (item.StatusText || item.Status || "-").toString();
                    var isReady = statusText.toUpperCase() === "READY";
                    var badgeClass = isReady
                        ? "assign-tech-detail-status ready"
                        : "assign-tech-detail-status";
                    html.push("<tr>"
                        + "<td>" + escapeHtml(item.DeviceID || "-") + "</td>"
                        + "<td>" + escapeHtml(item.NoSN || "-") + "</td>"
                        + "<td>" + escapeHtml(item.DeviceTypeDesc || techDeviceDetailState.deviceTypeDesc || "-") + "</td>"
                        + "<td><span class=\"" + badgeClass + "\">" + escapeHtml(statusText) + "</span></td>"
                        + "</tr>");
                }

                body.innerHTML = html.join("");
            }

            function closeTechDeviceDetailModal() {
                var backdrop = getTechDeviceDetailBackdrop();
                if (backdrop) {
                    backdrop.classList.remove("open");
                }
                techDeviceDetailState.searchText = "";
                var input = document.getElementById("assignTechDeviceSearchInput");
                if (input) {
                    input.value = "";
                }
            }

            function openTechDeviceDetailModal(params) {
                return;
            }

            function openTechStockModal(trigger) {
                return;
            }

            function closeTechStockModal() {
                var backdrop = getTechStockBackdrop();
                if (backdrop) {
                    backdrop.classList.remove("open");
                }
                closeTechDeviceDetailModal();
            }

            function handleTechSummaryAction(event) {
                var isKeyboard = event.type === "keydown";
                if (isKeyboard && event.key !== "Enter" && event.key !== " ") {
                    return;
                }

                var dayTotalTrigger = closestByClass(event.target, "assign-day-total-trigger");
                if (dayTotalTrigger) {
                    if (isKeyboard) {
                        event.preventDefault();
                    }
                    event.stopPropagation();
                    openDayTotalJoModal(dayTotalTrigger);
                    return;
                }

                var totalTrigger = closestByClass(event.target, "tech-totaljob-trigger");
                if (totalTrigger) {
                    if (isKeyboard) {
                        event.preventDefault();
                    }
                    openTotalJobModal(totalTrigger);
                    return;
                }

                var stockTrigger = closestByClass(event.target, "tech-stock-trigger");
                if (stockTrigger) {
                    if (isKeyboard) {
                        event.preventDefault();
                    }
                    return;
                }
            }

            function setFeedback(text, isError, isSuccess) {
                var feedback = document.getElementById("assignJobFeedback");
                if (!feedback) {
                    return;
                }
                feedback.classList.remove("error");
                feedback.classList.remove("success");
                feedback.classList.remove("is-visible");
                feedback.textContent = text || "";
                if (isError) {
                    feedback.classList.add("error");
                }
                if (isSuccess) {
                    feedback.classList.add("success");
                }
                if (text) {
                    feedback.classList.add("is-visible");
                }
            }

            function setStatusOnlyFeedback(text, isError, isSuccess) {
                var feedback = document.getElementById("assignStatusFeedback");
                if (!feedback) {
                    return;
                }
                feedback.classList.remove("error");
                feedback.classList.remove("success");
                feedback.classList.remove("is-visible");
                feedback.textContent = text || "";
                if (isError) {
                    feedback.classList.add("error");
                }
                if (isSuccess) {
                    feedback.classList.add("success");
                }
                if (text) {
                    feedback.classList.add("is-visible");
                }
            }

            function setStatusOnlySubmitLoading(isLoading) {
                var submitBtn = document.getElementById("assignStatusSubmitBtn");
                statusOnlyModalState.isSaving = !!isLoading;
                if (!submitBtn) {
                    return;
                }
                submitBtn.disabled = !!isLoading;
                submitBtn.classList.toggle("is-loading", !!isLoading);
                submitBtn.textContent = isLoading ? "Saving..." : "Ubah Status";
            }

            function resolveStatusOnlyTargetStatus(currentStatus) {
                var normalizedCurrent = normalizeStatusCode(currentStatus || "");
                var candidates = ["AV", "OF", "CT", "IZ"];
                for (var i = 0; i < candidates.length; i++) {
                    if (candidates[i] !== normalizedCurrent) {
                        return candidates[i];
                    }
                }
                return "AV";
            }

            function setActiveStatusOnlyButton() {
                var buttons = document.querySelectorAll("#assignStatusOnlyWrap .assign-status-btn");
                var currentStatus = normalizeStatusCode(statusOnlyModalState.currentStatus || "");
                var targetStatus = normalizeStatusCode(statusOnlyModalState.targetStatus || "");
                for (var i = 0; i < buttons.length; i++) {
                    var button = buttons[i];
                    var statusValue = normalizeStatusCode(button.getAttribute("data-status-value") || "");
                    var isCurrentStatus = statusValue === currentStatus;
                    button.disabled = isCurrentStatus;
                    if (isCurrentStatus) {
                        button.setAttribute("aria-disabled", "true");
                    } else {
                        button.removeAttribute("aria-disabled");
                    }
                    button.classList.toggle("active", statusValue === targetStatus);
                }
            }

            function closeStatusOnlyModal() {
                var backdrop = getStatusOnlyBackdrop();
                if (backdrop) {
                    backdrop.classList.remove("open");
                }
                statusOnlyModalState.activeCell = null;
                statusOnlyModalState.technicianId = "";
                statusOnlyModalState.technicianName = "";
                statusOnlyModalState.schDate = "";
                statusOnlyModalState.currentStatus = "";
                statusOnlyModalState.targetStatus = "";
                setStatusOnlySubmitLoading(false);
                setStatusOnlyFeedback("", false, false);
            }

            function openStatusOnlyModal(cell) {
                if (!isStatusOnlyEditableCell(cell)) {
                    return;
                }
                if (!requireValidItId(cell, true)) {
                    return;
                }

                var technicianId = (cell.getAttribute("data-tech-id") || "").trim();
                var dateText = (cell.getAttribute("data-date") || "").trim();
                if (!technicianId || !dateText) {
                    return;
                }

                var rawStatus = (cell.getAttribute("data-status") || "").toUpperCase();
                var currentStatus = normalizeStatusCode(rawStatus);
                if (!currentStatus) {
                    currentStatus = "AV";
                }

                statusOnlyModalState.activeCell = cell;
                statusOnlyModalState.technicianId = technicianId;
                statusOnlyModalState.technicianName = cell.getAttribute("data-tech-name") || "-";
                statusOnlyModalState.schDate = dateText;
                statusOnlyModalState.currentStatus = currentStatus;
                statusOnlyModalState.targetStatus = resolveStatusOnlyTargetStatus(currentStatus);

                var subtitle = document.getElementById("assignStatusSubtitle");
                if (subtitle) {
                    subtitle.textContent = "IT Support: " + technicianId;
                }
                var infoTech = document.getElementById("assignStatusInfoTechName");
                if (infoTech) {
                    infoTech.textContent = statusOnlyModalState.technicianName;
                }
                var infoDate = document.getElementById("assignStatusInfoDate");
                if (infoDate) {
                    infoDate.textContent = formatDisplayDate(dateText);
                }
                var infoStatus = document.getElementById("assignStatusInfoCurrentStatus");
                if (infoStatus) {
                    infoStatus.textContent = currentStatus;
                }

                setActiveStatusOnlyButton();
                setStatusOnlySubmitLoading(false);
                setStatusOnlyFeedback("", false, false);

                var backdrop = getStatusOnlyBackdrop();
                if (backdrop) {
                    backdrop.classList.add("open");
                }
            }

            function showAssignToast(message, isError) {
                var toast = document.getElementById("assignToast");
                if (!toast) {
                    return;
                }

                toast.classList.remove("success");
                toast.classList.remove("error");
                toast.textContent = message || "";
                toast.classList.add(isError ? "error" : "success");
                toast.classList.add("show");

                window.setTimeout(function () {
                    toast.classList.remove("show");
                }, 2600);
            }

            function setAssignSubmitLoading(isLoading) {
                var submitBtn = document.getElementById("assignJobSubmitBtn");
                var administrationBtn = document.getElementById("assignJobAdministrationBtn");
                var isAssignAction = assignModalState.submitAction !== "administration";
                var administrationLabel = assignModalState.administrationTargetStatus === "UD"
                    ? "Un Administration"
                    : "Is Administration";

                if (submitBtn) {
                    submitBtn.disabled = !!isLoading;
                    submitBtn.classList.toggle("is-loading", !!isLoading && isAssignAction);
                    submitBtn.textContent = (isLoading && isAssignAction) ? "Saving..." : "Assign";
                }
                if (administrationBtn) {
                    administrationBtn.disabled = !!isLoading;
                    administrationBtn.classList.toggle("is-loading", !!isLoading && !isAssignAction);
                    administrationBtn.textContent = (isLoading && !isAssignAction) ? "Saving..." : administrationLabel;
                }
                assignModalState.isSaving = !!isLoading;
            }

            function getAssignPageUrl() {
                var path = window.location.pathname || "dashboard_assign_job_itsupport.aspx";
                return path.substring(path.lastIndexOf("/") + 1) || "dashboard_assign_job_itsupport.aspx";
            }

            function callAssignPageMethod(methodName, payload, onSuccess, onError) {
                var request = new XMLHttpRequest();
                request.open(
                    "POST",
                    getAssignPageUrl()
                        + "?action=wm&method=" + encodeURIComponent(methodName || ""),
                    true);
                request.setRequestHeader("Content-Type", "application/json; charset=utf-8");
                request.setRequestHeader("Accept", "application/json");
                request.onreadystatechange = function () {
                    if (request.readyState !== 4) {
                        return;
                    }

                    if (request.status !== 200) {
                        if (typeof onError === "function") {
                            onError("HTTP " + request.status);
                        }
                        return;
                    }

                    try {
                        var text = String(request.responseText || "").replace(/^\uFEFF/, "").trim();
                        if (!text || text.charAt(0) === "<") {
                            throw new Error("Invalid response");
                        }
                        var response = JSON.parse(text);
                        if (response && typeof response.d !== "undefined") {
                            response = typeof response.d === "string" ? JSON.parse(response.d) : response.d;
                        }
                        if (typeof onSuccess === "function") {
                            onSuccess(response || {});
                        }
                    } catch (ex) {
                        if (typeof onError === "function") {
                            onError("Format response tidak valid.");
                        }
                    }
                };
                request.send(JSON.stringify(payload || {}));
            }

            function updateCellVisualAfterSave(displayValue, canAssign, targetCell, remainingJoInfo) {
                var cell = targetCell || assignModalState.activeCell;
                if (!cell) {
                    return;
                }

                var value = (displayValue || "").toString().trim();
                if (!value) {
                    value = "AV";
                }

                cell.textContent = value;
                cell.setAttribute("data-status", value);

                cell.classList.remove("st-av", "st-unit", "st-unit-open", "st-unit-done", "st-close", "st-off", "st-cuti", "st-sakit", "st-izin", "st-unavailable", "day-today-status");
                if (/^\d+$/.test(value)) {
                    var hasRemainingJo = remainingJoInfo && remainingJoInfo.hasRemainingJo;
                    var remainingJo = remainingJoInfo ? remainingJoInfo.remainingJo : -1;
                    var isRemainingJoDone = hasRemainingJo && remainingJo === 0;
                    cell.classList.add(isRemainingJoDone ? "st-unit-done" : "st-unit-open");
                } else if (value.toUpperCase() === "AV" || value.toUpperCase() === "AD") {
                    cell.classList.add("st-av");
                } else {
                    cell.classList.add("st-off");
                }

                syncTodayCellClasses(cell, value);

                var upperValue = normalizeStatusCode(value);
                var cellDate = parseIsoDate(cell.getAttribute("data-date"));
                var isFutureOrToday = cellDate && cellDate.getTime() >= getTodayDateOnly().getTime();
                var isAssignableClickable = !getIsTechnicianUser() && isFutureOrToday && (
                    upperValue === "OF"
                    || (!!canAssign && (upperValue === "AV" || upperValue === "AD"))
                );
                var isStatusOnlyClickable = isStatusOnlyEditableCell(cell);
                var isClickable = isAssignableClickable || isStatusOnlyClickable;
                var isReportClickable = isNumericStatus(value);
                cell.classList.toggle("assign-cell--clickable", isClickable);
                cell.classList.toggle("assign-cell--report-clickable", isReportClickable);
                cell.classList.toggle("assign-cell--disabled", !(isClickable || isReportClickable));
                cell.setAttribute("data-can-assign", isClickable ? "1" : "0");
                cell.setAttribute("aria-disabled", (isClickable || isReportClickable) ? "false" : "true");
                cell.setAttribute("tabindex", (isClickable || isReportClickable) ? "0" : "-1");
            }

            function refreshAvailabilityAfterSave(technicianId, scheduleDate, fallbackDisplay, callback, targetCell, extraTechnicianIds) {
                var techIds = [];
                var seen = {};
                function addTechId(id) {
                    id = (id || "").trim();
                    if (!id || seen[id]) {
                        return;
                    }
                    seen[id] = true;
                    techIds.push(id);
                }

                addTechId(technicianId);
                if (extraTechnicianIds && extraTechnicianIds.length) {
                    for (var i = 0; i < extraTechnicianIds.length; i++) {
                        addTechId(extraTechnicianIds[i]);
                    }
                }
                if (!techIds.length) {
                    techIds.push((technicianId || "").trim());
                }

                var pending = techIds.length;
                function finishOne() {
                    pending--;
                    if (pending <= 0 && typeof callback === "function") {
                        callback();
                    }
                }

                for (var j = 0; j < techIds.length; j++) {
                    (function (techId) {
                        var cellForTech = targetCell
                            && normalizeTechId(targetCell.getAttribute("data-tech-id")) === normalizeTechId(techId)
                            ? targetCell
                            : findScheduleCell(techId, scheduleDate);

                        callAssignPageMethod(
                            "RefreshAvailability",
                            {
                                technicianId: techId || "",
                                schDate: scheduleDate || ""
                            },
                            function (result) {
                                applyRefreshAvailabilityResult(result, techId, scheduleDate, fallbackDisplay, cellForTech);
                                finishOne();
                            },
                            function () {
                                applyRefreshAvailabilityResult(null, techId, scheduleDate, fallbackDisplay, cellForTech);
                                finishOne();
                            });
                    })(techIds[j]);
                }
            }

            function getJoInfoBackdrop() {
                return document.getElementById("assignJoInfoBackdrop");
            }

            function escapeHtml(text) {
                return String(text == null ? "" : text)
                    .replace(/&/g, "&amp;")
                    .replace(/</g, "&lt;")
                    .replace(/>/g, "&gt;")
                    .replace(/"/g, "&quot;")
                    .replace(/'/g, "&#39;");
            }

            function renderJoExpandableText(value) {
                var text = String(value == null || value === "" ? "-" : value);
                var safe = escapeHtml(text);
                if (text === "-") {
                    return "<span class=\"assign-jo-text-plain\">" + safe + "</span>";
                }
                return "<div class=\"assign-jo-expandable\">"
                    + "<span class=\"assign-jo-text-content\">" + safe + "</span>"
                    + "<button type=\"button\" class=\"assign-jo-text-toggle\" aria-expanded=\"false\" hidden>Show</button>"
                    + "</div>";
            }

            function syncJoExpandableTextToggles(root) {
                var run = function () {
                    var scope = root || document;
                    var wraps = scope.querySelectorAll ? scope.querySelectorAll(".assign-jo-expandable") : [];
                    for (var i = 0; i < wraps.length; i++) {
                        var wrap = wraps[i];
                        var content = wrap.querySelector(".assign-jo-text-content");
                        var toggle = wrap.querySelector(".assign-jo-text-toggle");
                        if (!content || !toggle) {
                            continue;
                        }

                        wrap.classList.remove("is-expanded", "has-overflow");
                        toggle.setAttribute("aria-expanded", "false");
                        toggle.textContent = "Show";
                        toggle.hidden = true;

                        if (content.scrollWidth > content.clientWidth + 1) {
                            wrap.classList.add("has-overflow");
                            toggle.hidden = false;
                        }
                    }
                };

                if (window.requestAnimationFrame) {
                    window.requestAnimationFrame(function () {
                        window.requestAnimationFrame(run);
                    });
                } else {
                    setTimeout(run, 0);
                }
            }

            function normalizeJoTypeForApi(value) {
                var type = (value || "").toLowerCase();
                if (type === "installation" || type === "new_install" || type === "new installation") {
                    return "installation";
                }
                return "maintenance";
            }

            function isScheduleRowCompleted(row) {
                var statusCode = ((row && row.StatusCode) || "").toUpperCase();
                var statusText = ((row && row.StatusText) || "").toLowerCase();
                return statusCode === "CL" || statusText === "selesai";
            }

            function isScheduleRowNewInstall(row) {
                if (!row) {
                    return false;
                }
                var jobType = String(row.JobType || "").trim();
                if (!jobType || jobType === "-") {
                    return true;
                }
                return normalizeJoTypeForApi(jobType) === "installation";
            }

            function buildInstallationJobNewUrl(jobId) {
                var id = String(jobId || "").trim().replace(/;$/, "");
                if (!id || id === "-") {
                    return "";
                }
                return assignNewInstallPageUrl + "?JobID=" + encodeURIComponent(id);
            }

            function normalizeJoTypeForButton(value) {
                var type = normalizeJoTypeForApi(value);
                return type === "installation" ? "new_install" : "maintenance";
            }

            function normalizeDeviceGroup(value) {
                return (value || "").toString().trim().toUpperCase() === "ACS" ? "ACS" : "GPS";
            }

            function getRemainingUnitByDeviceGroup(order, deviceGroupId) {
                if (!order) {
                    return 0;
                }

                var normalizedGroup = normalizeDeviceGroup(deviceGroupId);
                if (normalizedGroup === "ACS") {
                    return toInt(order.RemainingUnitAcs, 0);
                }
                return toInt(order.RemainingUnitGps, 0);
            }

            function getDeviceGroupDisplayText(deviceGroupId) {
                return normalizeDeviceGroup(deviceGroupId) === "ACS" ? "ACS" : "GPS";
            }

            function getCustomerGpsCount(order) {
                return toInt(order && order.CustomerGpsCount, 0);
            }

            function getCustomerAcsCount(order) {
                return toInt(order && order.CustomerAcsCount, 0);
            }

            function getJobOrderTransferLabel(order, targetTechnicianId) {
                if (!requiresTransferNoteForOrder(order, targetTechnicianId)) {
                    return "";
                }
                var itName = (order.AssignedTechnicianName || order.AssignedTechnicianId || "-").toString().trim();
                return "Pindah dari IT Support: " + itName;
            }

            function getInsDeviceTypeId(joType, order) {
                if (normalizeJoTypeForApi(joType) !== "installation") {
                    return "";
                }
                if (!order) {
                    return "";
                }
                return (order.DeviceTypeID || "").toString().trim();
            }

            function getJoInfoTableColspan() {
                return 14;
            }

            function renderJoBranchFilterOptions(branchOptions, selectedValue) {
                var select = document.getElementById("assignJoBranchFilter");
                if (!select) {
                    return;
                }
                var selected = (selectedValue || "").trim();
                var options = ["<option value=\"\">SEMUA Branch</option>"];
                var rows = branchOptions || [];
                for (var i = 0; i < rows.length; i++) {
                    var branchName = (rows[i] || "").toString().trim();
                    if (!branchName) {
                        continue;
                    }
                    var isSelected = branchName === selected;
                    options.push("<option value=\"" + escapeHtml(branchName) + "\"" + (isSelected ? " selected" : "") + ">" + escapeHtml(branchName) + "</option>");
                }
                select.innerHTML = options.join("");
            }

            function applySelectedOrderAreaFromCustomer(order, stateObject) {
                if (!order || !stateObject) {
                    return;
                }
                var defaultAreaId = normalizeAreaId(order.DefaultAreaId || "");
                if (defaultAreaId) {
                    stateObject.selectedAreaId = defaultAreaId;
                    if (stateObject.supAreaId !== defaultAreaId) {
                        stateObject.supAreaId = defaultAreaId;
                        fetchAreaOptions(defaultAreaId, function () {
                            if (isCompletedReportModalOpen()) {
                                syncReportAreaDropdown();
                            } else {
                                syncAssignAreaDropdown();
                            }
                        });
                    }
                }
            }

            function syncJoDeviceTypeColumnVisibility() {
                var showDeviceType = normalizeJoTypeForApi(assignModalState.joType) === "installation";
                var headers = document.querySelectorAll(".assign-jo-device-type-col");
                for (var i = 0; i < headers.length; i++) {
                    headers[i].style.display = showDeviceType ? "" : "none";
                }
            }

            function showJoLoadingRow() {
                var body = document.getElementById("assignJoTableBody");
                if (!body) {
                    return;
                }
                body.innerHTML = "<tr><td class=\"assign-jo-loading\" colspan=\"" + getJoInfoTableColspan() + "\">Loading job order...</td></tr>";
            }

            function getCurrentJoRows() {
                if (!assignModalState.joRows || !assignModalState.joRows.length) {
                    return [];
                }
                return assignModalState.joRows;
            }

            function fetchJobOrderInformation() {
                assignModalState.joLoading = true;
                showJoLoadingRow();

                var url = getAssignPageUrl()
                    + "?action=load_job_order"
                    + "&activeTab=all"
                    + "&searchKeyword=" + encodeURIComponent(assignModalState.joSearchText || "")
                    + "&branchFilter=" + encodeURIComponent(assignModalState.joBranchFilter || "")
                    + "&pageIndex=" + encodeURIComponent(String(assignModalState.joPage || 1))
                    + "&pageSize=" + encodeURIComponent(String(assignJoPageSize || 20));

                var request = new XMLHttpRequest();
                request.open("GET", url, true);
                request.setRequestHeader("Accept", "application/json");
                request.onreadystatechange = function () {
                    if (request.readyState !== 4) {
                        return;
                    }

                    assignModalState.joLoading = false;
                    try {
                        var text = String(request.responseText || "").replace(/^\uFEFF/, "").trim();
                        if (!text) {
                            throw new Error("Response kosong dari server.");
                        }
                        if (text.charAt(0) === "<") {
                            throw new Error("Server mengembalikan HTML (session/login/compile).");
                        }

                        var data = JSON.parse(text);
                        if (data && typeof data.d !== "undefined") {
                            data = typeof data.d === "string" ? JSON.parse(data.d) : data.d;
                        }

                        assignModalState.joRows = (data && data.Rows) ? data.Rows : [];
                        assignModalState.joTotalPages = (data && data.TotalPages) ? data.TotalPages : 1;
                        assignModalState.joTotalRecords = (data && data.TotalRecords) ? data.TotalRecords : 0;
                        assignModalState.joPage = (data && data.PageIndex) ? data.PageIndex : 1;
                        assignModalState.joBranchOptions = (data && data.BranchOptions) ? data.BranchOptions : [];
                        renderJoBranchFilterOptions(assignModalState.joBranchOptions, assignModalState.joBranchFilter);
                        renderJoInfoTable((data && data.ErrorMessage) ? data.ErrorMessage : "");
                    } catch (ex) {
                        assignModalState.joRows = [];
                        assignModalState.joTotalPages = 1;
                        assignModalState.joTotalRecords = 0;
                        var detail = (ex && ex.message) ? ex.message : "Format respons data tidak valid.";
                        if (request.status && request.status !== 200) {
                            detail = "HTTP " + request.status + " - " + detail;
                        }
                        renderJoInfoTable(detail);
                    }
                };
                request.send(null);
            }

            function getSelectedOrder() {
                return assignModalState.selectedOrder;
            }

            function getCustomerDisplayText(order) {
                if (!order) {
                    return "-";
                }

                var customerId = order.Customer || "";
                var customerName = order.CustomerName || "";
                if (customerId && customerName) {
                    return customerId + " - " + customerName;
                }
                return customerId || customerName || "-";
            }

            function renderSelectedJoText() {
                var element = document.getElementById("assignPickedJoText");
                if (!element) {
                    return;
                }
                var selected = getSelectedOrder();
                if (!selected) {
                    element.textContent = "Belum ada Job Order dipilih";
                    return;
                }
                var transferLabel = getJobOrderTransferLabel(selected, getTargetAssignTechnicianId("main"));
                element.textContent = selected.JobID + " - " + getCustomerDisplayText(selected) + " (" + selected.BranchName + ")";
                if (transferLabel) {
                    element.textContent += " [" + transferLabel + "]";
                }
            }

            function renderOrderInfo() {
                var selected = getSelectedOrder();
                var customer = document.getElementById("assignInfoCustomer");
                var remainingGps = document.getElementById("assignInfoRemainingGps");
                var remainingAcs = document.getElementById("assignInfoRemainingAcs");
                var inputUnit = document.getElementById("assignInputUnit");
                var areaSelect = document.getElementById("assignAreaSelect");
                var selectedGroup = normalizeDeviceGroup(assignModalState.deviceGroupId);
                var selectedGroupRemaining = getRemainingUnitByDeviceGroup(selected, selectedGroup);
                syncAssignAreaDropdown();

                if (!selected) {
                    if (customer) {
                        customer.textContent = "-";
                    }
                    if (remainingGps) {
                        remainingGps.textContent = "0";
                    }
                    if (remainingAcs) {
                        remainingAcs.textContent = "0";
                    }
                    if (inputUnit) {
                        inputUnit.value = "1";
                        inputUnit.max = "1";
                    }
                    syncAssignInputByDeviceGroup(0);
                    renderSelectedJoText();
                    syncTransferNoteField("assignTransferNoteWrap", "assignTransferNoteInput", null, getTargetAssignTechnicianId("main"));
                    if (areaSelect && normalizeAreaId(areaSelect.value) !== normalizeAreaId(assignModalState.selectedAreaId)) {
                        areaSelect.value = normalizeAreaId(assignModalState.selectedAreaId);
                    }
                    return;
                }

                if (customer) {
                    customer.textContent = getCustomerDisplayText(selected);
                }
                var customerGps = getCustomerGpsCount(selected);
                var customerAcs = getCustomerAcsCount(selected);
                var assignedItSupport = toInt(selected.TotalAssign, 0);
                if (remainingGps) {
                    remainingGps.textContent = customerGps.toString();
                }
                if (remainingAcs) {
                    remainingAcs.textContent = customerAcs.toString();
                }
                if (inputUnit) {
                    inputUnit.value = "1";
                    inputUnit.max = "1";
                }
                syncAssignInputByDeviceGroup(assignedItSupport);
                renderSelectedJoText();
                syncTransferNoteField("assignTransferNoteWrap", "assignTransferNoteInput", selected, getTargetAssignTechnicianId("main"));
                if (areaSelect && normalizeAreaId(areaSelect.value) !== normalizeAreaId(assignModalState.selectedAreaId)) {
                    areaSelect.value = normalizeAreaId(assignModalState.selectedAreaId);
                }
            }

            function syncAssignInputByDeviceGroup(assignedUnit) {
                var inputUnit = document.getElementById("assignInputUnit");
                var inputLabel = document.getElementById("assignInputUnitLabel");
                var inputHint = document.getElementById("assignInputUnitHint");
                var safeAssigned = Math.max(0, toInt(assignedUnit, 0));

                if (inputLabel) {
                    inputLabel.textContent = "Assign IT Support";
                }

                if (inputUnit) {
                    inputUnit.value = "1";
                    inputUnit.placeholder = "1";
                    inputUnit.max = "1";
                }

                if (inputHint) {
                    inputHint.textContent = "Sudah Assign IT Support : " + safeAssigned.toString();
                }
            }

            function validateAssignUnitInput(showFeedback) {
                var selectedStatus = (assignModalState.targetStatus || "AV").toUpperCase();
                if (selectedStatus !== "AV") {
                    return true;
                }

                var order = getSelectedOrder();
                if (!order) {
                    if (showFeedback) {
                        setFeedback("Job Order wajib dipilih untuk status Available.", true, false);
                    }
                    return false;
                }

                var selectedGroup = normalizeDeviceGroup(assignModalState.deviceGroupId);
                var groupText = getDeviceGroupDisplayText(selectedGroup);
                var remainingUnit = getRemainingUnitByDeviceGroup(order, selectedGroup);

                if (remainingUnit <= 0 && !isJobOrderTransfer(order, getTargetAssignTechnicianId("main"))) {
                    if (showFeedback) {
                        var assignedCount = toInt(order.TotalAssign, 0);
                        if (assignedCount > 0) {
                            setFeedback("Job Order ini sudah di-assign ke IT Support. Cari Job ID tepat di picker untuk pindah.", true, false);
                        } else {
                            setFeedback("Job Order tidak bisa di-assign saat ini.", true, false);
                        }
                    }
                    return false;
                }

                if (requiresTransferNoteForOrder(order, getTargetAssignTechnicianId("main"))) {
                    var transferNote = getDashboardNoteValue("assignTransferNoteInput");
                    if (!transferNote) {
                        if (showFeedback) {
                            setFeedback("Catatan pindah IT Support wajib diisi.", true, false);
                            var transferInput = document.getElementById("assignTransferNoteInput");
                            if (transferInput) {
                                transferInput.focus();
                            }
                        }
                        return false;
                    }
                    if (showFeedback) {
                        setFeedback(getJobOrderTransferLabel(order, getTargetAssignTechnicianId("main")), false, false);
                    }
                }

                return true;
            }

            function renderJoInfoTable(errorMessage) {
                var body = document.getElementById("assignJoTableBody");
                var pagesWrap = document.getElementById("assignJoPageNumbers");
                var prevBtn = document.getElementById("assignJoPrevBtn");
                var nextBtn = document.getElementById("assignJoNextBtn");
                if (!body || !pagesWrap || !prevBtn || !nextBtn) {
                    return;
                }

                syncJoDeviceTypeColumnVisibility();
                var rowsData = getCurrentJoRows();
                var totalPages = Math.max(1, assignModalState.joTotalPages || 1);
                var showDeviceType = true;
                var tableColspan = getJoInfoTableColspan();

                if (errorMessage) {
                    body.innerHTML = "<tr><td class=\"assign-jo-empty\" colspan=\"" + tableColspan + "\">" + escapeHtml(errorMessage) + "</td></tr>";
                } else if (!rowsData.length) {
                    body.innerHTML = "<tr><td class=\"assign-jo-empty\" colspan=\"" + tableColspan + "\">Data job order tidak ditemukan.</td></tr>";
                } else {
                    var rows = [];
                    for (var i = 0; i < rowsData.length; i++) {
                        var item = rowsData[i];
                        var customerGps = getCustomerGpsCount(item);
                        var lastAssign = item.LastAssignDate || "-";
                        var assignDate = item.AssignDate || "-";
                        var slaDays = toInt(item.SlaDays, 0);
                        var transfer = isJobOrderTransferCandidate(item);
                        var pickLabel = transfer ? "Pindah" : "Pilih";
                        var pickClass = transfer ? "assign-jo-pick-btn assign-jo-transfer-btn" : "assign-jo-pick-btn";
                        var transferHint = transfer
                            ? "<div class=\"assign-jo-transfer-hint\">" + escapeHtml(getJobOrderTransferLabel(item, getTargetAssignTechnicianId(joLookupOwner === "report" ? "report" : "main"))) + "</div>"
                            : "";
                        var deviceTypeCell = showDeviceType
                            ? "<td class=\"assign-jo-device-type-col\">" + escapeHtml(item.DeviceTypeDesc || "-") + "</td>"
                            : "";
                        rows.push("<tr>" +
                            "<td>" + escapeHtml(item.JobID) + transferHint + "</td>" +
                            "<td class=\"assign-jo-text-col\">" + renderJoExpandableText(getCustomerDisplayText(item)) + "</td>" +
                            "<td class=\"assign-jo-text-col\">" + renderJoExpandableText(item.BranchName || "-") + "</td>" +
                            "<td class=\"assign-jo-text-col\">" + renderJoExpandableText(item.Address || "-") + "</td>" +
                            "<td>" + escapeHtml(item.PicName || "-") + "</td>" +
                            "<td>" + escapeHtml(item.CustomerNumber || item.PicPhone || "-") + "</td>" +
                            "<td class=\"assign-jo-text-col\">" + renderJoExpandableText(item.MarketingName || "-") + "</td>" +
                            "<td class=\"assign-jo-text-col\">" + renderJoExpandableText(item.Remark || "-") + "</td>" +
                            deviceTypeCell +
                            "<td>" + escapeHtml(customerGps.toString()) + "</td>" +
                            "<td>" + escapeHtml(lastAssign) + "</td>" +
                            "<td>" + escapeHtml(assignDate) + "</td>" +
                            "<td>" + escapeHtml(slaDays.toString()) + "</td>" +
                            "<td><button type=\"button\" class=\"" + pickClass + "\" data-pick-index=\"" + i + "\">" + pickLabel + "</button></td>" +
                            "</tr>");
                    }
                    body.innerHTML = rows.join("");
                    syncJoExpandableTextToggles(body);
                }

                prevBtn.disabled = assignModalState.joPage <= 1;
                nextBtn.disabled = assignModalState.joPage >= totalPages;

                var pageButtons = [];
                var currentPage = assignModalState.joPage;
                var maxVisible = 5;
                var startPage = Math.max(1, currentPage - 3);
                var endPage = Math.min(totalPages, startPage + maxVisible - 1);
                if ((endPage - startPage + 1) < maxVisible) {
                    startPage = Math.max(1, endPage - maxVisible + 1);
                }

                if (startPage > 1) {
                    pageButtons.push("<button type=\"button\" class=\"assign-jo-page-btn\" data-page=\"1\">1</button>");
                    if (startPage > 2) {
                        pageButtons.push("<span class=\"assign-jo-page-ellipsis\">...</span>");
                    }
                }

                for (var page = startPage; page <= endPage; page++) {
                    pageButtons.push("<button type=\"button\" class=\"assign-jo-page-btn" + (page === currentPage ? " active" : "") + "\" data-page=\"" + page + "\">" + page + "</button>");
                }

                if (endPage < totalPages) {
                    if (endPage < totalPages - 1) {
                        pageButtons.push("<span class=\"assign-jo-page-ellipsis\">...</span>");
                    }
                    pageButtons.push("<button type=\"button\" class=\"assign-jo-page-btn\" data-page=\"" + totalPages + "\">" + totalPages + "</button>");
                }
                pagesWrap.innerHTML = pageButtons.join("");
            }

            function openJoInfoModal(owner, joTypeValue) {
                joLookupOwner = owner || "main";
                if (joTypeValue) {
                    assignModalState.joType = normalizeJoTypeForButton(joTypeValue);
                }
                if (joLookupOwner === "report") {
                    assignModalState.selectedOrder = reportModalState.selectedOrder;
                    assignModalState.deviceGroupId = reportModalState.deviceGroupId;
                }
                assignModalState.joPage = 1;
                assignModalState.joSearchText = "";
                var input = document.getElementById("assignJoSearchInput");
                if (input) {
                    input.value = "";
                }
                syncJoDeviceTypeColumnVisibility();
                fetchJobOrderInformation();
                var backdrop = getJoInfoBackdrop();
                if (backdrop) {
                    backdrop.classList.add("open");
                }
            }

            function closeJoInfoModal() {
                var backdrop = getJoInfoBackdrop();
                if (backdrop) {
                    backdrop.classList.remove("open");
                }
            }

            function setActiveJoTypeButton() {
                var buttons = document.querySelectorAll("#assignJobTypeWrap .assign-jo-type-btn");
                for (var i = 0; i < buttons.length; i++) {
                    var button = buttons[i];
                    button.classList.toggle("active", normalizeJoTypeForButton(button.getAttribute("data-jo-type")) === assignModalState.joType);
                }
            }

            function setActiveStatusButton() {
                var buttons = document.querySelectorAll("#assignStatusWrap .assign-status-btn");
                for (var i = 0; i < buttons.length; i++) {
                    var button = buttons[i];
                    button.classList.toggle("active", button.getAttribute("data-status-value") === assignModalState.targetStatus);
                }
            }

            function setActiveDeviceGroupButton() {
                var buttons = document.querySelectorAll("#assignDeviceGroupWrap .assign-jo-type-btn");
                for (var i = 0; i < buttons.length; i++) {
                    var button = buttons[i];
                    button.classList.toggle("active", normalizeDeviceGroup(button.getAttribute("data-device-group")) === assignModalState.deviceGroupId);
                }
            }

            function syncDeviceGroupAvailabilityByJoType() {
                var isMaintenance = normalizeJoTypeForApi(assignModalState.joType) === "maintenance";
                var buttons = document.querySelectorAll("#assignDeviceGroupWrap .assign-jo-type-btn");
                for (var i = 0; i < buttons.length; i++) {
                    var button = buttons[i];
                    var group = normalizeDeviceGroup(button.getAttribute("data-device-group"));
                    var disableButton = isMaintenance && group === "ACS";
                    button.disabled = disableButton;
                    if (disableButton) {
                        button.setAttribute("aria-disabled", "true");
                    } else {
                        button.removeAttribute("aria-disabled");
                    }
                }

                if (isMaintenance && assignModalState.deviceGroupId !== "GPS") {
                    assignModalState.deviceGroupId = "GPS";
                }
            }

            function closeAssignModal() {
                var backdrop = getBackdrop();
                if (backdrop) {
                    backdrop.classList.remove("open");
                }
                closeJoInfoModal();
                assignModalState.activeCell = null;
                setAssignSubmitLoading(false);
                setFeedback("", false, false);
            }

            function openAssignModal(cell) {
                if (!isAssignableCell(cell)) {
                    return;
                }
                if (!requireValidItId(cell, true)) {
                    return;
                }

                assignModalState.activeCell = cell;
                assignModalState.joType = "new_install";
                assignModalState.deviceGroupId = "GPS";
                assignModalState.targetStatus = "AV";
                assignModalState.administrationTargetStatus = "AD";
                assignModalState.supAreaId = (cell.getAttribute("data-sup-area") || "").trim();
                assignModalState.selectedAreaId = "";
                assignModalState.selectedOrder = null;
                clearDashboardNote("assignTransferNoteInput");
                assignModalState.submitAction = "assign";
                assignModalState.joRows = [];
                assignModalState.joTotalPages = 1;
                assignModalState.joTotalRecords = 0;

                var techName = cell.getAttribute("data-tech-name") || "-";
                var dateText = cell.getAttribute("data-date") || "-";
                var status = (cell.getAttribute("data-status") || "AV").toUpperCase();
                if (status === "AD") {
                    assignModalState.administrationTargetStatus = "UD";
                }
                var subtitle = document.getElementById("assignJobSubtitle");
                if (subtitle) {
                    subtitle.textContent = "IT Support: " + (cell.getAttribute("data-tech-id") || "-");
                }

                setAssignSubmitLoading(false);
                var infoTech = document.getElementById("assignInfoTechName");
                var infoDate = document.getElementById("assignInfoDate");
                var infoStatus = document.getElementById("assignInfoCurrentStatus");
                if (infoTech) {
                    infoTech.textContent = techName;
                }
                if (infoDate) {
                    infoDate.textContent = formatDisplayDate(dateText);
                }
                if (infoStatus) {
                    infoStatus.textContent = status;
                }

                setActiveJoTypeButton();
                syncDeviceGroupAvailabilityByJoType();
                setActiveDeviceGroupButton();
                setActiveStatusButton();
                renderOrderInfo();
                setFeedback("", false, false);

                var backdrop = getBackdrop();
                if (backdrop) {
                    backdrop.classList.add("open");
                }

                fetchAreaOptions(assignModalState.supAreaId, function () {
                    if (!normalizeAreaId(assignModalState.selectedAreaId) && areaLookupState.rows.length) {
                        assignModalState.selectedAreaId = normalizeAreaId(areaLookupState.rows[0].AreaID);
                    }
                    renderOrderInfo();
                });
            }

            function setCreateJoFeedback(message, isError, isSuccess) {
                var box = document.getElementById("assignCreateJoFeedback");
                if (!box) {
                    return;
                }
                box.classList.remove("error");
                box.classList.remove("success");
                box.classList.remove("is-visible");
                box.textContent = message || "";
                if (isError) {
                    box.classList.add("error");
                }
                if (isSuccess) {
                    box.classList.add("success");
                }
                if (message) {
                    box.classList.add("is-visible");
                }
            }

            function getCreateJoBackdrop() {
                return document.getElementById("assignCreateJoBackdrop");
            }

            function isCreateJoModalOpen() {
                var backdrop = getCreateJoBackdrop();
                return !!(backdrop && backdrop.classList.contains("open"));
            }

            function closeCreateJoModal() {
                var backdrop = getCreateJoBackdrop();
                if (backdrop) {
                    backdrop.classList.remove("open");
                }
                document.body.classList.remove("assign-create-jo-open");
                if (window.jQuery) {
                    $("#modal-training-customer").modal("hide");
                }
                setCreateJoFeedback("", false, false);
            }

            function openCreateJoModal() {
                if (getIsTechnicianUser()) {
                    showAssignToast("User IT Support tidak dapat membuat Job Training/Visit.", true);
                    return;
                }

                resetTrainingAssignForm("", "", formatIsoDateInput(new Date()));
                setCreateJoFeedback("", false, false);
                var backdrop = getCreateJoBackdrop();
                if (backdrop) {
                    backdrop.classList.add("open");
                }
                document.body.classList.add("assign-create-jo-open");
                ensureTrainingLookupsLoaded(function () {
                    if (!trainingLookupState.loaded) {
                        setCreateJoFeedback("Gagal memuat category / billable.", true, false);
                    }
                });
            }

            function syncCreateJoButtonVisibility() {
                var btn = document.getElementById("assignCreateJoBtn");
                if (!btn) {
                    return;
                }
                btn.style.display = getIsTechnicianUser() ? "none" : "";
            }

            function resetTrainingAssignForm(techId, techName, schDate) {
                var custId = document.getElementById("assignTrainingCustId");
                var itUserId = document.getElementById("assignTrainingItUserId");
                var itUserName = document.getElementById("assignTrainingItUserName");
                var schHidden = document.getElementById("assignTrainingSchDate");
                var reqDate = document.getElementById("assignTrainingReqDate");
                var schInput = document.getElementById("assignTrainingSchDateInput");
                var billable = document.getElementById("assignTrainingBillable");
                var category = document.getElementById("assignTrainingCategory");
                var remark = document.getElementById("assignTrainingRemark");
                var picName = document.getElementById("assignTrainingPicName");
                var picPhone = document.getElementById("assignTrainingPicPhone");
                var picked = document.getElementById("assignPickedCustomerText");
                var custName = document.getElementById("assignTrainingCustName");
                var custType = document.getElementById("assignTrainingCustType");
                var custBranch = document.getElementById("assignTrainingCustBranch");

                if (custId) custId.value = "";
                if (itUserId) itUserId.value = techId || "";
                if (itUserName) itUserName.value = techName || "";
                if (schHidden) schHidden.value = schDate || "";
                if (reqDate) reqDate.value = formatIsoDateInput(new Date());
                if (schInput) schInput.value = schDate || "";
                if (billable) billable.value = "";
                if (category) category.value = "";
                if (remark) remark.value = "";
                if (picName) picName.value = "";
                if (picPhone) picPhone.value = "";
                if (picked) picked.textContent = "Belum ada Customer dipilih";
                if (custName) custName.textContent = "-";
                if (custType) custType.textContent = "-";
                if (custBranch) custBranch.textContent = "-";
            }

            function formatIsoDateInput(dateObj) {
                if (!dateObj || isNaN(dateObj.getTime())) {
                    return "";
                }
                var y = dateObj.getFullYear();
                var m = ("0" + (dateObj.getMonth() + 1)).slice(-2);
                var d = ("0" + dateObj.getDate()).slice(-2);
                return y + "-" + m + "-" + d;
            }

            var trainingLookupState = {
                loaded: false,
                loading: false
            };
            var createJoSaving = false;

            function ensureTrainingLookupsLoaded(done) {
                function lookupFeedback(message, isError, isSuccess) {
                    if (isCreateJoModalOpen()) {
                        setCreateJoFeedback(message, isError, isSuccess);
                    } else {
                        setFeedback(message, isError, isSuccess);
                    }
                }

                if (trainingLookupState.loaded) {
                    if (typeof done === "function") done();
                    return;
                }
                if (trainingLookupState.loading) {
                    if (typeof done === "function") done();
                    return;
                }
                trainingLookupState.loading = true;
                lookupFeedback("Memuat category / billable...", false, false);
                callAssignPageMethod("LoadTrainingLookups", {}, function (result) {
                    trainingLookupState.loading = false;
                    var isSuccess = ((result && result.Result) || "").toUpperCase() === "SUCCESS";
                    if (!isSuccess) {
                        lookupFeedback((result && result.Message) || "Gagal memuat lookup Training/Visit.", true, false);
                        if (typeof done === "function") done();
                        return;
                    }
                    fillTrainingSelect("assignTrainingCategory", result.Categories || []);
                    fillTrainingSelect("assignTrainingBillable", result.Billables || []);
                    trainingLookupState.loaded = true;
                    lookupFeedback("", false, false);
                    if (typeof done === "function") done();
                }, function (errorMessage) {
                    trainingLookupState.loading = false;
                    lookupFeedback("Gagal memuat lookup. " + (errorMessage || ""), true, false);
                    if (typeof done === "function") done();
                });
            }

            function fillTrainingSelect(selectId, items) {
                var select = document.getElementById(selectId);
                if (!select) {
                    return;
                }
                var html = '<option value="">[Select]</option>';
                for (var i = 0; i < items.length; i++) {
                    var item = items[i] || {};
                    var value = item.Value || item.value || "";
                    var text = item.Text || item.text || value;
                    if (!value) {
                        continue;
                    }
                    html += '<option value="' + escapeHtml(value) + '">' + escapeHtml(text) + '</option>';
                }
                select.innerHTML = html;
            }

            function submitTrainingAssignFromModal() {
                if (createJoSaving) {
                    return true;
                }

                var custId = (document.getElementById("assignTrainingCustId") || {}).value || "";
                var reqDate = (document.getElementById("assignTrainingReqDate") || {}).value || "";
                var schDate = (document.getElementById("assignTrainingSchDateInput") || {}).value
                    || (document.getElementById("assignTrainingSchDate") || {}).value
                    || "";
                var billableId = (document.getElementById("assignTrainingBillable") || {}).value || "";
                var categoryId = (document.getElementById("assignTrainingCategory") || {}).value || "";
                var remark = (document.getElementById("assignTrainingRemark") || {}).value || "";
                var itUserId = (document.getElementById("assignTrainingItUserId") || {}).value || "";
                var itUserName = (document.getElementById("assignTrainingItUserName") || {}).value || "";
                var submitBtn = document.getElementById("assignCreateJoSubmitBtn");

                if (!custId) {
                    setCreateJoFeedback("Customer wajib dipilih.", true, false);
                    return true;
                }
                if (!categoryId || categoryId === "[Select]") {
                    setCreateJoFeedback("Category Training/Visit wajib dipilih.", true, false);
                    return true;
                }
                if (!billableId || billableId === "[Select]") {
                    setCreateJoFeedback("Billable wajib dipilih.", true, false);
                    return true;
                }
                if (!schDate) {
                    setCreateJoFeedback("Schedule Date wajib diisi.", true, false);
                    return true;
                }

                createJoSaving = true;
                if (submitBtn) {
                    submitBtn.disabled = true;
                }
                setCreateJoFeedback("Menyimpan job training/visit...", false, false);
                callAssignPageMethod(
                    "SaveJobTrainingAssign",
                    {
                        custId: custId,
                        reqDate: reqDate,
                        billableId: billableId,
                        schDate: schDate,
                        remark: remark,
                        categoryId: categoryId,
                        itUserId: itUserId,
                        itUserName: itUserName
                    },
                    function (result) {
                        createJoSaving = false;
                        if (submitBtn) {
                            submitBtn.disabled = false;
                        }
                        var isSuccess = ((result && result.Result) || "").toUpperCase() === "SUCCESS";
                        if (!isSuccess) {
                            var failedMessage = (result && result.Message) || "Gagal submit job training.";
                            setCreateJoFeedback(failedMessage, true, false);
                            showAssignToast(failedMessage, true);
                            return;
                        }

                        var successMessage = (result && result.Message) || "Job Training/Visit berhasil dibuat.";
                        setCreateJoFeedback(successMessage, false, true);
                        showAssignToast(successMessage, false);
                        closeCreateJoModal();
                    },
                    function (errorMessage) {
                        createJoSaving = false;
                        if (submitBtn) {
                            submitBtn.disabled = false;
                        }
                        setCreateJoFeedback("Gagal submit job training. " + (errorMessage || ""), true, false);
                        showAssignToast("Gagal submit job training.", true);
                    });
                return true;
            }

            window.postCustChild = function (sCustID, sFullName, sCustTypeDesc, sBranchName, sPICName, sPICPhone) {
                if (!sCustID) {
                    return;
                }
                var custId = document.getElementById("assignTrainingCustId");
                var picked = document.getElementById("assignPickedCustomerText");
                var custName = document.getElementById("assignTrainingCustName");
                var custType = document.getElementById("assignTrainingCustType");
                var custBranch = document.getElementById("assignTrainingCustBranch");
                var picName = document.getElementById("assignTrainingPicName");
                var picPhone = document.getElementById("assignTrainingPicPhone");
                if (custId) custId.value = sCustID;
                if (picked) picked.textContent = sCustID + " - " + (sFullName || "");
                if (custName) custName.textContent = sFullName || "-";
                if (custType) custType.textContent = sCustTypeDesc || "-";
                if (custBranch) custBranch.textContent = sBranchName || "-";
                if (picName) picName.value = sPICName || "";
                if (picPhone) picPhone.value = sPICPhone || "";
                if (window.jQuery) {
                    $("#modal-training-customer").modal("hide");
                }
                if (isCreateJoModalOpen()) {
                    setCreateJoFeedback("", false, false);
                } else {
                    setFeedback("", false, false);
                }
            };

            function decorateAssignCells() {
                var cells = document.querySelectorAll(".assign-cell-trigger");
                for (var i = 0; i < cells.length; i++) {
                    var cell = cells[i];
                    var assignEnabled = isAssignableCell(cell);
                    var statusOnlyEnabled = !assignEnabled && isStatusOnlyEditableCell(cell);
                    var reportEnabled = isCompletedReportCell(cell);
                    var enabled = assignEnabled || statusOnlyEnabled || reportEnabled;
                    cell.classList.toggle("assign-cell--clickable", assignEnabled || statusOnlyEnabled);
                    cell.classList.toggle("assign-cell--report-clickable", reportEnabled);
                    cell.classList.toggle("assign-cell--disabled", !enabled);
                    cell.setAttribute("data-can-assign", (assignEnabled || statusOnlyEnabled) ? "1" : "0");
                    cell.setAttribute("aria-disabled", enabled ? "false" : "true");
                    cell.setAttribute("tabindex", enabled ? "0" : "-1");
                }
            }

            function handleCellAction(event) {
                var isKeyboard = event.type === "keydown";
                if (isKeyboard && event.key !== "Enter" && event.key !== " ") {
                    return;
                }

                var cell = closestByClass(event.target, "assign-cell-trigger");
                if (!cell) {
                    return;
                }

                if (isKeyboard) {
                    event.preventDefault();
                }
                if (isAssignableCell(cell)) {
                    if (!requireValidItId(cell, true)) {
                        return;
                    }
                    openAssignModal(cell);
                    return;
                }

                if (isStatusOnlyEditableCell(cell)) {
                    openStatusOnlyModal(cell);
                    return;
                }

                if (isCompletedReportCell(cell)) {
                    openCompletedReportModal(cell);
                }
            }

            function findScheduleCell(technicianId, scheduleDate) {
                var wantTech = normalizeTechId(technicianId);
                var wantDate = normalizeScheduleDateText(scheduleDate);
                if (!wantTech || !wantDate) {
                    return null;
                }

                var cells = document.querySelectorAll(".assign-cell-trigger[data-tech-id][data-date]");
                for (var i = 0; i < cells.length; i++) {
                    var cell = cells[i];
                    if (normalizeTechId(cell.getAttribute("data-tech-id")) === wantTech
                        && sameScheduleDate(cell.getAttribute("data-date"), wantDate)) {
                        return cell;
                    }
                }
                return null;
            }

            function updateDayTotalButtonLabel(scheduleDate, dayTotalJo) {
                var wantDate = normalizeScheduleDateText(scheduleDate);
                if (!wantDate) {
                    return;
                }

                var total = parseInt(dayTotalJo, 10);
                if (isNaN(total) || total < 0) {
                    total = 0;
                }

                var buttons = document.querySelectorAll(".assign-day-total-trigger");
                for (var i = 0; i < buttons.length; i++) {
                    var btn = buttons[i];
                    if (!sameScheduleDate(btn.getAttribute("data-date"), wantDate)) {
                        continue;
                    }
                    btn.setAttribute("data-total-jo", String(total));
                    btn.textContent = String(total);
                    btn.classList.toggle("is-zero", total <= 0);
                    btn.setAttribute("title", "Total JO: " + total);
                }
            }

            function applyRefreshAvailabilityResult(result, technicianId, scheduleDate, fallbackDisplay, targetCell) {
                var isSuccess = result && (result.Result || "").toUpperCase() === "SUCCESS";
                var cell = targetCell || findScheduleCell(technicianId, scheduleDate);
                if (isSuccess) {
                    updateCellVisualAfterSave(
                        result.DisplayValue || fallbackDisplay,
                        result.CanAssign,
                        cell,
                        {
                            hasRemainingJo: !!result.HasRemainingJo,
                            remainingJo: parseInt(result.RemainingJo, 10)
                        });
                    if (result.HasDayTotalJo) {
                        updateDayTotalButtonLabel(scheduleDate, result.DayTotalJo);
                    }
                    return;
                }

                if (cell) {
                    updateCellVisualAfterSave(fallbackDisplay, false, cell);
                }
            }

            function findAnyScheduleCellForTech(technicianId) {
                var wantTech = normalizeTechId(technicianId);
                if (!wantTech) {
                    return null;
                }

                var cells = document.querySelectorAll(".assign-cell-trigger[data-tech-id]");
                for (var i = 0; i < cells.length; i++) {
                    if (normalizeTechId(cells[i].getAttribute("data-tech-id")) === wantTech) {
                        return cells[i];
                    }
                }
                return null;
            }

            function updateTotalJoAssignCell(technicianId, totalJoAssign, scheduleDate) {
                var anchor = findScheduleCell(technicianId, scheduleDate) || findAnyScheduleCellForTech(technicianId);
                if (!anchor || !anchor.closest) {
                    return;
                }

                var row = anchor.closest("tr");
                if (!row) {
                    return;
                }

                var col = row.querySelector(".col-total-assign");
                if (!col) {
                    return;
                }

                var total = parseInt(totalJoAssign, 10);
                col.textContent = String(isNaN(total) || total < 0 ? 0 : total);
            }

            function applyScheduleDayStateResult(result, scheduleDate, primaryCell) {
                if (!result || (result.Result || "").toUpperCase() !== "SUCCESS") {
                    return false;
                }

                updateDayTotalButtonLabel(scheduleDate, result.DayTotalJo);
                var cells = result.Cells || [];
                for (var i = 0; i < cells.length; i++) {
                    var item = cells[i] || {};
                    var techId = (item.TechnicianId || "").trim();
                    if (!techId) {
                        continue;
                    }

                    var cell = primaryCell
                        && normalizeTechId(primaryCell.getAttribute("data-tech-id")) === normalizeTechId(techId)
                        ? primaryCell
                        : findScheduleCell(techId, scheduleDate);
                    updateCellVisualAfterSave(item.DisplayValue || "AV", !!item.CanAssign, cell);
                    updateTotalJoAssignCell(techId, item.TotalJoAssign, scheduleDate);
                }
                return true;
            }

            function refreshScheduleDayState(scheduleDate, technicianIds, primaryCell, callback) {
                var ids = [];
                var seen = {};
                (technicianIds || []).forEach(function (id) {
                    id = (id || "").trim();
                    if (!id || seen[id]) {
                        return;
                    }
                    seen[id] = true;
                    ids.push(id);
                });

                function finishFallback() {
                    refreshAvailabilityAfterSave(
                        ids.length ? ids[0] : "",
                        scheduleDate,
                        "AV",
                        function () {
                            callAssignPageMethod(
                                "GetDayTotalJoList",
                                { scheduleDate: scheduleDate },
                                function (dayResult) {
                                    if (dayResult && (dayResult.Result || "").toUpperCase() === "SUCCESS") {
                                        updateDayTotalButtonLabel(scheduleDate, dayResult.TotalJO);
                                    }
                                    if (typeof callback === "function") {
                                        callback();
                                    }
                                },
                                callback);
                        },
                        primaryCell,
                        ids.slice(1));
                }

                if (!scheduleDate) {
                    if (typeof callback === "function") {
                        callback();
                    }
                    return;
                }

                callAssignPageMethod(
                    "RefreshScheduleDayState",
                    {
                        schDate: scheduleDate,
                        technicianIds: ids
                    },
                    function (result) {
                        if (applyScheduleDayStateResult(result, scheduleDate, primaryCell)) {
                            if (typeof callback === "function") {
                                callback();
                            }
                            return;
                        }
                        finishFallback();
                    },
                    finishFallback);
            }

            function syncDayTotalButtonLabels() {
                var buttons = document.querySelectorAll(".assign-day-total-trigger");
                for (var i = 0; i < buttons.length; i++) {
                    var btn = buttons[i];
                    var totalJo = btn.getAttribute("data-total-jo");
                    if (totalJo != null && totalJo !== "") {
                        btn.textContent = totalJo;
                    }
                }
            }

            function bindAssignDocumentEvents() {
                if (assignDocumentEventsBound) {
                    return;
                }
                assignDocumentEventsBound = true;
                syncDayTotalButtonLabels();

                document.addEventListener("click", handleCellAction);
                document.addEventListener("keydown", handleCellAction);
                document.addEventListener("click", handleTechSummaryAction);
                document.addEventListener("keydown", handleTechSummaryAction);
                document.addEventListener("click", function (event) {
                    var closeButton = closestByClass(event.target, "assign-job-close");
                    if (closeButton) {
                        if (closeButton.id === "assignJoInfoCloseBtn") {
                            closeJoInfoModal();
                        } else if (closeButton.id === "assignStatusCloseBtn") {
                            closeStatusOnlyModal();
                        } else if (closeButton.id === "assignCreateJoCloseBtn") {
                            closeCreateJoModal();
                        } else {
                            closeAssignModal();
                        }
                        return;
                    }

                    var reportCloseButton = closestByClass(event.target, "assign-report-close");
                    if (reportCloseButton) {
                        closeCompletedReportModal();
                        return;
                    }

                    if (event.target && (event.target.id === "assignReportRemarkCloseBtn" || event.target.id === "assignReportRemarkCloseActionBtn")) {
                        closeReportRemarkModal();
                        return;
                    }

                    var cancelButton = event.target && event.target.id === "assignJobCancelBtn"
                        ? event.target
                        : closestByClass(event.target, "assign-action-btn");
                    if (cancelButton && cancelButton.id === "assignJobCancelBtn") {
                        closeAssignModal();
                        return;
                    }

                    if (event.target && event.target.id === "assignReportCloseActionBtn") {
                        closeCompletedReportModal();
                        return;
                    }
                    if (event.target && event.target.id === "assignStatusCloseActionBtn") {
                        closeStatusOnlyModal();
                        return;
                    }

                    var reportDetailBtn = closestByClass(event.target, "assign-report-training-btn")
                        || closestByClass(event.target, "assign-report-detail-btn");
                    if (reportDetailBtn) {
                        var reportDetailIndex = parseInt(reportDetailBtn.getAttribute("data-row-index"), 10);
                        if (!isNaN(reportDetailIndex) && reportModalState.rows && reportDetailIndex >= 0 && reportDetailIndex < reportModalState.rows.length) {
                            openReportRemarkModal(reportModalState.rows[reportDetailIndex] || {});
                        }
                        return;
                    }

                    var stockDeviceCard = closestByClass(event.target, "assign-stock-device-card");
                    if (stockDeviceCard) {
                        openTechDeviceDetailModal({
                            technicianId: stockDeviceCard.getAttribute("data-tech-id") || "",
                            technicianName: stockDeviceCard.getAttribute("data-tech-name") || techStockState.technicianName || "",
                            deviceTypeId: stockDeviceCard.getAttribute("data-device-type-id") || "",
                            deviceTypeDesc: stockDeviceCard.getAttribute("data-device-type-desc") || ""
                        });
                        return;
                    }

                    if (event.target && (event.target.id === "assignTechStockCloseBtn" || event.target.id === "assignTechStockCloseActionBtn")) {
                        closeTechStockModal();
                        return;
                    }

                    if (event.target && (event.target.id === "assignTechDeviceDetailCloseBtn" || event.target.id === "assignTechDeviceDetailCloseActionBtn")) {
                        closeTechDeviceDetailModal();
                    }
                });

                // Do not close on backdrop click. Close only via explicit actions.
                document.addEventListener("keydown", function (event) {
                    if (event.key === "Escape") {
                        if (reportModalState.pendingDeleteRowIndex >= 0 && isCompletedReportModalOpen()) {
                            cancelReportDeletePanel();
                            return;
                        }
                        var reportRemarkBackdrop = getReportRemarkBackdrop();
                        if (reportRemarkBackdrop && reportRemarkBackdrop.classList.contains("open")) {
                            closeReportRemarkModal();
                            return;
                        }
                        var reportBackdrop = getCompletedReportBackdrop();
                        if (reportBackdrop && reportBackdrop.classList.contains("open")) {
                            closeCompletedReportModal();
                            return;
                        }
                        if (isTotalJobModalOpen()) {
                            closeTotalJobModal();
                            return;
                        }
                        var stockDetailBackdrop = getTechDeviceDetailBackdrop();
                        if (stockDetailBackdrop && stockDetailBackdrop.classList.contains("open")) {
                            closeTechDeviceDetailModal();
                            return;
                        }
                        var stockBackdrop = getTechStockBackdrop();
                        if (stockBackdrop && stockBackdrop.classList.contains("open")) {
                            closeTechStockModal();
                            return;
                        }
                        var statusBackdrop = getStatusOnlyBackdrop();
                        if (statusBackdrop && statusBackdrop.classList.contains("open")) {
                            closeStatusOnlyModal();
                            return;
                        }
                        var joBackdrop = getJoInfoBackdrop();
                        if (joBackdrop && joBackdrop.classList.contains("open")) {
                            closeJoInfoModal();
                            return;
                        }
                        if (isCreateJoModalOpen()) {
                            closeCreateJoModal();
                            return;
                        }
                        closeAssignModal();
                    }
                });
            }

            function bindAssignModalControls() {
                var closeBtn = document.getElementById("assignJobCloseBtn");
                var closeActionBtn = document.getElementById("assignJobCloseActionBtn");
                var cancelBtn = document.getElementById("assignJobCancelBtn");
                var submitBtn = document.getElementById("assignJobSubmitBtn");
                var backdrop = getBackdrop();
                var joTypeWrap = document.getElementById("assignJobTypeWrap");
                var deviceGroupWrap = document.getElementById("assignDeviceGroupWrap");
                var statusWrap = document.getElementById("assignStatusWrap");
                var pickJobBtn = document.getElementById("assignPickJobOrderBtn");
                var joInfoCloseBtn = document.getElementById("assignJoInfoCloseBtn");
                var joInfoBackdrop = getJoInfoBackdrop();
                var reportCloseBtn = document.getElementById("assignReportCloseBtn");
                var reportCloseActionBtn = document.getElementById("assignReportCloseActionBtn");
                var reportTableBody = document.getElementById("assignReportTableBody");
                var reportJobTypeWrap = document.getElementById("assignReportJobTypeWrap");
                var reportDeviceGroupWrap = document.getElementById("assignReportDeviceGroupWrap");
                var reportStatusWrap = document.getElementById("assignReportStatusWrap");
                var reportPickJobBtn = document.getElementById("assignReportPickJobOrderBtn");
                var reportInputUnit = document.getElementById("assignReportInputUnit");
                var reportAreaSelect = document.getElementById("assignReportAreaSelect");
                var reportAssignBtn = document.getElementById("assignReportAssignBtn");
                var reportAddBtn = document.getElementById("assignReportAddBtn");
                var reportCancelAddBtn = document.getElementById("assignReportCancelAddBtn");
                var techStockCloseBtn = document.getElementById("assignTechStockCloseBtn");
                var techStockCloseActionBtn = document.getElementById("assignTechStockCloseActionBtn");
                var techDetailCloseBtn = document.getElementById("assignTechDeviceDetailCloseBtn");
                var techDetailCloseActionBtn = document.getElementById("assignTechDeviceDetailCloseActionBtn");
                var reportRemarkCloseBtn = document.getElementById("assignReportRemarkCloseBtn");
                var reportRemarkCloseActionBtn = document.getElementById("assignReportRemarkCloseActionBtn");
                var techDetailSearchInput = document.getElementById("assignTechDeviceSearchInput");
                var joSearchInput = document.getElementById("assignJoSearchInput");
                var joSearchBtn = document.getElementById("assignJoSearchBtn");
                var joTableBody = document.getElementById("assignJoTableBody");
                var joPrevBtn = document.getElementById("assignJoPrevBtn");
                var joNextBtn = document.getElementById("assignJoNextBtn");
                var joPageNumbers = document.getElementById("assignJoPageNumbers");
                var assignInputUnit = document.getElementById("assignInputUnit");
                var assignAreaSelect = document.getElementById("assignAreaSelect");
                var administrationBtn = document.getElementById("assignJobAdministrationBtn");
                var statusOnlyCloseBtn = document.getElementById("assignStatusCloseBtn");
                var statusOnlyCloseActionBtn = document.getElementById("assignStatusCloseActionBtn");
                var statusOnlyCancelBtn = document.getElementById("assignStatusCancelBtn");
                var statusOnlySubmitBtn = document.getElementById("assignStatusSubmitBtn");
                var statusOnlyWrap = document.getElementById("assignStatusOnlyWrap");

                if (closeBtn) {
                    closeBtn.addEventListener("click", closeAssignModal);
                }
                if (closeActionBtn) {
                    closeActionBtn.addEventListener("click", closeAssignModal);
                }
                if (cancelBtn) {
                    cancelBtn.addEventListener("click", closeAssignModal);
                }

                var createJoBtn = document.getElementById("assignCreateJoBtn");
                var createJoCloseBtn = document.getElementById("assignCreateJoCloseBtn");
                var createJoCancelBtn = document.getElementById("assignCreateJoCancelBtn");
                var createJoSubmitBtn = document.getElementById("assignCreateJoSubmitBtn");
                var createJoPickCustomerBtn = document.getElementById("assignCreateJoPickCustomerBtn");
                if (createJoBtn && !createJoBtn.getAttribute("data-bound")) {
                    createJoBtn.addEventListener("click", function (event) {
                        event.preventDefault();
                        openCreateJoModal();
                    });
                    createJoBtn.setAttribute("data-bound", "1");
                }
                if (createJoCloseBtn && !createJoCloseBtn.getAttribute("data-bound")) {
                    createJoCloseBtn.addEventListener("click", function (event) {
                        event.preventDefault();
                        closeCreateJoModal();
                    });
                    createJoCloseBtn.setAttribute("data-bound", "1");
                }
                if (createJoCancelBtn && !createJoCancelBtn.getAttribute("data-bound")) {
                    createJoCancelBtn.addEventListener("click", function (event) {
                        event.preventDefault();
                        closeCreateJoModal();
                    });
                    createJoCancelBtn.setAttribute("data-bound", "1");
                }
                if (createJoSubmitBtn && !createJoSubmitBtn.getAttribute("data-bound")) {
                    createJoSubmitBtn.addEventListener("click", function (event) {
                        event.preventDefault();
                        submitTrainingAssignFromModal();
                    });
                    createJoSubmitBtn.setAttribute("data-bound", "1");
                }
                if (createJoPickCustomerBtn && !createJoPickCustomerBtn.getAttribute("data-bound")) {
                    createJoPickCustomerBtn.addEventListener("click", function (event) {
                        event.preventDefault();
                        if (window.jQuery) {
                            $("#modal-training-customer").modal("show");
                        }
                    });
                    createJoPickCustomerBtn.setAttribute("data-bound", "1");
                }
                syncCreateJoButtonVisibility();
                if (statusOnlyCloseBtn) {
                    statusOnlyCloseBtn.addEventListener("click", closeStatusOnlyModal);
                }
                if (statusOnlyCloseActionBtn) {
                    statusOnlyCloseActionBtn.addEventListener("click", closeStatusOnlyModal);
                }
                if (statusOnlyCancelBtn) {
                    statusOnlyCancelBtn.addEventListener("click", closeStatusOnlyModal);
                }
                if (statusOnlyWrap) {
                    statusOnlyWrap.addEventListener("click", function (event) {
                        var button = closestByClass(event.target, "assign-status-btn");
                        if (!button || button.disabled) {
                            return;
                        }
                        statusOnlyModalState.targetStatus = normalizeStatusCode(button.getAttribute("data-status-value") || "");
                        setActiveStatusOnlyButton();
                        setStatusOnlyFeedback("", false, false);
                    });
                }
                if (statusOnlySubmitBtn) {
                    statusOnlySubmitBtn.addEventListener("click", function () {
                        if (statusOnlyModalState.isSaving) {
                            return;
                        }

                        var activeCell = statusOnlyModalState.activeCell;
                        var technicianId = statusOnlyModalState.technicianId || (activeCell ? (activeCell.getAttribute("data-tech-id") || "") : "");
                        var scheduleDate = statusOnlyModalState.schDate || (activeCell ? (activeCell.getAttribute("data-date") || "") : "");
                        var targetStatus = normalizeStatusCode(statusOnlyModalState.targetStatus || "");
                        var currentStatus = normalizeStatusCode(statusOnlyModalState.currentStatus || "");

                        if (!technicianId) {
                            setStatusOnlyFeedback("IT Support tidak valid.", true, false);
                            return;
                        }
                        if (!scheduleDate) {
                            setStatusOnlyFeedback("Tanggal schedule tidak valid.", true, false);
                            return;
                        }
                        if (!targetStatus || targetStatus === currentStatus) {
                            setStatusOnlyFeedback("Pilih status yang berbeda dari status saat ini.", true, false);
                            return;
                        }

                        setStatusOnlySubmitLoading(true);
                        setStatusOnlyFeedback("Menyimpan perubahan status...", false, false);

                        callAssignPageMethod(
                            "UpdateTechnicianStatus",
                            {
                                technicianId: technicianId,
                                schDate: scheduleDate,
                                targetStatus: targetStatus
                            },
                            function (result) {
                                var isSuccess = (result.Result || "").toUpperCase() === "SUCCESS";
                                if (!isSuccess) {
                                    setStatusOnlySubmitLoading(false);
                                    var failedMessage = result.Message || "Gagal mengubah status.";
                                    setStatusOnlyFeedback(failedMessage, true, false);
                                    showAssignToast(failedMessage, true);
                                    return;
                                }

                                var successMessage = result.Message || "Status berhasil diubah.";
                                setStatusOnlyFeedback(successMessage, false, true);
                                showAssignToast(successMessage, false);

                                refreshAvailabilityAfterSave(technicianId, scheduleDate, targetStatus, function () {
                                    setStatusOnlySubmitLoading(false);
                                    closeStatusOnlyModal();
                                    window.location.reload();
                                }, activeCell);
                            },
                            function (errorMessage) {
                                setStatusOnlySubmitLoading(false);
                                setStatusOnlyFeedback("Gagal mengubah status. " + (errorMessage || ""), true, false);
                                showAssignToast("Gagal mengubah status.", true);
                            });
                    });
                }
                if (pickJobBtn) {
                    pickJobBtn.addEventListener("click", function () {
                        openJoInfoModal("main", assignModalState.joType);
                    });
                }
                if (joInfoCloseBtn) {
                    joInfoCloseBtn.addEventListener("click", function () {
                        closeJoInfoModal();
                    });
                }
                if (reportCloseBtn) {
                    reportCloseBtn.addEventListener("click", closeCompletedReportModal);
                }
                if (reportCloseActionBtn) {
                    reportCloseActionBtn.addEventListener("click", closeCompletedReportModal);
                }
                if (reportTableBody) {
                    reportTableBody.addEventListener("click", function (event) {
                        var editBtn = closestByClass(event.target, "assign-report-edit-btn");
                        if (editBtn) {
                            var editIndex = parseInt(editBtn.getAttribute("data-row-index"), 10);
                            if (!isNaN(editIndex)) {
                                editAssignFromReportRow(editIndex);
                            }
                            return;
                        }

                        var deleteBtn = closestByClass(event.target, "assign-report-delete-btn");
                        if (!deleteBtn) {
                            return;
                        }
                        var rowIndex = parseInt(deleteBtn.getAttribute("data-row-index"), 10);
                        if (isNaN(rowIndex)) {
                            return;
                        }
                        deleteAssignFromReportRow(rowIndex);
                    });
                }
                if (reportJobTypeWrap) {
                    reportJobTypeWrap.addEventListener("click", function (event) {
                        var button = closestByClass(event.target, "assign-jo-type-btn");
                        if (!button) {
                            return;
                        }
                        reportModalState.joType = normalizeJoTypeForButton(button.getAttribute("data-jo-type"));
                        reportModalState.selectedOrder = null;
                        renderReportAssignForm();
                        setReportFeedback("", false, false);
                    });
                }
                if (reportDeviceGroupWrap) {
                    reportDeviceGroupWrap.addEventListener("click", function (event) {
                        var button = closestByClass(event.target, "assign-jo-type-btn");
                        if (!button || button.disabled) {
                            return;
                        }
                        reportModalState.deviceGroupId = normalizeDeviceGroup(button.getAttribute("data-device-group"));
                        renderReportAssignForm();
                        validateReportAssignInput(false);
                        setReportFeedback("", false, false);
                    });
                }
                if (reportStatusWrap) {
                    reportStatusWrap.addEventListener("click", function (event) {
                        var button = closestByClass(event.target, "assign-status-btn");
                        if (!button || button.disabled) {
                            return;
                        }
                        reportModalState.targetStatus = button.getAttribute("data-status-value") || "AV";
                        var reportStatusButtons = document.querySelectorAll("#assignReportStatusWrap .assign-status-btn");
                        for (var iReportStatus = 0; iReportStatus < reportStatusButtons.length; iReportStatus++) {
                            var statusBtn = reportStatusButtons[iReportStatus];
                            statusBtn.classList.toggle("active", statusBtn.getAttribute("data-status-value") === reportModalState.targetStatus);
                        }
                        setReportFeedback("", false, false);
                    });
                }
                if (reportPickJobBtn) {
                    reportPickJobBtn.addEventListener("click", function () {
                        openJoInfoModal("report", reportModalState.joType);
                    });
                }
                if (reportInputUnit) {
                    reportInputUnit.addEventListener("input", function () {
                        validateReportAssignInput(true);
                    });
                    reportInputUnit.addEventListener("blur", function () {
                        validateReportAssignInput(true);
                    });
                }
                if (reportAreaSelect) {
                    reportAreaSelect.addEventListener("change", function () {
                        reportModalState.selectedAreaId = normalizeAreaId(reportAreaSelect.value);
                        syncReportAreaDropdown();
                        setReportFeedback("", false, false);
                    });
                }
                if (reportAddBtn) {
                    reportAddBtn.addEventListener("click", openReportAssignCreator);
                }
                if (reportCancelAddBtn) {
                    reportCancelAddBtn.addEventListener("click", cancelReportAssignForm);
                }
                if (reportAssignBtn) {
                    reportAssignBtn.addEventListener("click", function () {
                        submitAssignFromReportModal("assign");
                    });
                }
                if (techStockCloseBtn) {
                    techStockCloseBtn.addEventListener("click", closeTechStockModal);
                }
                if (techStockCloseActionBtn) {
                    techStockCloseActionBtn.addEventListener("click", closeTechStockModal);
                }
                if (techDetailCloseBtn) {
                    techDetailCloseBtn.addEventListener("click", closeTechDeviceDetailModal);
                }
                if (techDetailCloseActionBtn) {
                    techDetailCloseActionBtn.addEventListener("click", closeTechDeviceDetailModal);
                }
                var reportDeleteConfirmBtn = document.getElementById("assignReportDeleteConfirmBtn");
                var reportDeleteCancelBtn = document.getElementById("assignReportDeleteCancelBtn");
                if (reportDeleteConfirmBtn) {
                    reportDeleteConfirmBtn.addEventListener("click", confirmReportDelete);
                }
                if (reportDeleteCancelBtn) {
                    reportDeleteCancelBtn.addEventListener("click", cancelReportDeletePanel);
                }
                if (reportRemarkCloseBtn) {
                    reportRemarkCloseBtn.addEventListener("click", closeReportRemarkModal);
                }
                if (reportRemarkCloseActionBtn) {
                    reportRemarkCloseActionBtn.addEventListener("click", closeReportRemarkModal);
                }
                if (techDetailSearchInput) {
                    techDetailSearchInput.addEventListener("input", function () {
                        techDeviceDetailState.searchText = techDetailSearchInput.value || "";
                        renderTechDeviceDetailRows();
                    });
                }
                // Do not close on backdrop click. Close only via explicit actions.
                if (joSearchBtn) {
                    joSearchBtn.addEventListener("click", function () {
                        assignModalState.joSearchText = joSearchInput ? joSearchInput.value : "";
                        assignModalState.joPage = 1;
                        fetchJobOrderInformation();
                    });
                }
                if (joSearchInput) {
                    joSearchInput.addEventListener("keydown", function (event) {
                        if (event.key === "Enter") {
                            event.preventDefault();
                            assignModalState.joSearchText = joSearchInput.value;
                            assignModalState.joPage = 1;
                            fetchJobOrderInformation();
                        }
                    });
                }
                var joBranchFilter = document.getElementById("assignJoBranchFilter");
                if (joBranchFilter) {
                    joBranchFilter.addEventListener("change", function () {
                        assignModalState.joBranchFilter = joBranchFilter.value || "";
                        assignModalState.joPage = 1;
                        fetchJobOrderInformation();
                    });
                }
                if (joTableBody) {
                    joTableBody.addEventListener("click", function (event) {
                        var toggleBtn = closestByClass(event.target, "assign-jo-text-toggle");
                        if (toggleBtn) {
                            event.preventDefault();
                            event.stopPropagation();
                            var wrap = toggleBtn.closest ? toggleBtn.closest(".assign-jo-expandable") : null;
                            if (!wrap && toggleBtn.parentNode) {
                                wrap = toggleBtn.parentNode;
                            }
                            if (wrap) {
                                var expanded = wrap.classList.toggle("is-expanded");
                                toggleBtn.setAttribute("aria-expanded", expanded ? "true" : "false");
                                toggleBtn.textContent = expanded ? "Hide" : "Show";
                            }
                            return;
                        }

                        var button = closestByClass(event.target, "assign-jo-pick-btn");
                        if (!button) {
                            return;
                        }
                        var index = parseInt(button.getAttribute("data-pick-index"), 10);
                        var rowsData = getCurrentJoRows();
                        if (isNaN(index) || index < 0 || index >= rowsData.length) {
                            return;
                        }
                        var selectedOrder = rowsData[index];
                        var shouldApplyToReport = joLookupOwner === "report" || isCompletedReportModalOpen();
                        applySelectedJobOrder(selectedOrder, shouldApplyToReport ? "report" : "main");
                        joLookupOwner = "main";
                        closeJoInfoModal();
                    });
                }
                if (joPrevBtn) {
                    joPrevBtn.addEventListener("click", function () {
                        if (assignModalState.joPage > 1) {
                            assignModalState.joPage--;
                            fetchJobOrderInformation();
                        }
                    });
                }
                if (joNextBtn) {
                    joNextBtn.addEventListener("click", function () {
                        if (assignModalState.joPage < assignModalState.joTotalPages) {
                            assignModalState.joPage++;
                            fetchJobOrderInformation();
                        }
                    });
                }
                if (joPageNumbers) {
                    joPageNumbers.addEventListener("click", function (event) {
                        var pageButton = closestByClass(event.target, "assign-jo-page-btn");
                        if (!pageButton) {
                            return;
                        }
                        var page = parseInt(pageButton.getAttribute("data-page"), 10);
                        if (isNaN(page)) {
                            return;
                        }
                        assignModalState.joPage = page;
                        fetchJobOrderInformation();
                    });
                }
                if (joTypeWrap) {
                    joTypeWrap.addEventListener("click", function (event) {
                        var button = closestByClass(event.target, "assign-jo-type-btn");
                        if (!button) {
                            return;
                        }
                        assignModalState.joType = normalizeJoTypeForButton(button.getAttribute("data-jo-type"));
                        assignModalState.selectedOrder = null;
                        assignModalState.joPage = 1;
                        assignModalState.joRows = [];
                        setActiveJoTypeButton();
                        syncDeviceGroupAvailabilityByJoType();
                        setActiveDeviceGroupButton();
                        renderOrderInfo();
                        setFeedback("", false, false);
                    });
                }
                if (statusWrap) {
                    statusWrap.addEventListener("click", function (event) {
                        var button = closestByClass(event.target, "assign-status-btn");
                        if (!button || button.disabled) {
                            return;
                        }
                        assignModalState.targetStatus = button.getAttribute("data-status-value") || "AV";
                        setActiveStatusButton();
                        setFeedback("", false, false);
                    });
                }
                if (deviceGroupWrap) {
                    deviceGroupWrap.addEventListener("click", function (event) {
                        var button = closestByClass(event.target, "assign-jo-type-btn");
                        if (!button) {
                            return;
                        }
                        if (button.disabled) {
                            return;
                        }
                        assignModalState.deviceGroupId = normalizeDeviceGroup(button.getAttribute("data-device-group"));
                        setActiveDeviceGroupButton();
                        renderOrderInfo();
                        if (assignModalState.joRows && assignModalState.joRows.length) {
                            renderJoInfoTable("");
                        }
                        validateAssignUnitInput(false);
                        setFeedback("", false, false);
                    });
                }
                if (assignInputUnit) {
                    assignInputUnit.addEventListener("input", function () {
                        validateAssignUnitInput(true);
                    });
                    assignInputUnit.addEventListener("blur", function () {
                        validateAssignUnitInput(true);
                    });
                }
                if (assignAreaSelect) {
                    assignAreaSelect.addEventListener("change", function () {
                        assignModalState.selectedAreaId = normalizeAreaId(assignAreaSelect.value);
                        syncAssignAreaDropdown();
                        setFeedback("", false, false);
                    });
                }
                if (submitBtn) {
                    submitBtn.addEventListener("click", function () {
                        assignModalState.submitAction = "assign";
                        if (assignModalState.isSaving) {
                            return;
                        }

                        var selectedStatus = (assignModalState.targetStatus || "AV").toUpperCase();
                        var isAvailableStatus = selectedStatus === "AV";
                        var selectedDeviceGroup = normalizeJoTypeForApi(assignModalState.joType) === "maintenance"
                            ? "GPS"
                            : normalizeDeviceGroup(assignModalState.deviceGroupId);
                        var order = getSelectedOrder();
                        if (isAvailableStatus && !order) {
                            setFeedback("Belum ada JO yang dapat dipilih.", true, false);
                            return;
                        }

                        var activeCell = assignModalState.activeCell;
                        if (!requireValidItId(activeCell, true)) {
                            return;
                        }
                        var technicianId = activeCell ? (activeCell.getAttribute("data-tech-id") || "") : "";
                        var scheduleDate = activeCell ? (activeCell.getAttribute("data-date") || "") : "";
                        if (!technicianId) {
                            setFeedback("IT Support tidak valid.", true, false);
                            return;
                        }
                        if (!scheduleDate) {
                            setFeedback("Tanggal schedule tidak valid.", true, false);
                            return;
                        }

                        var qty = 0;
                        var selectedAreaId = "";
                        var fallbackDisplay = selectedStatus;
                        if (isAvailableStatus) {
                            if (!validateAssignUnitInput(true)) {
                                return;
                            }
                            selectedAreaId = order ? normalizeAreaId(order.DefaultAreaId || "") : "";
                            qty = 1;
                            fallbackDisplay = "1";
                        }

                        function runMainAssignSave(assignRemark) {
                            setAssignSubmitLoading(true);
                            setFeedback("Menyimpan assignment...", false, false);

                            callAssignPageMethod(
                                "SaveAssignJob",
                                {
                                    assignId: "",
                                    jobId: order ? (order.JobID || "") : "",
                                    custId: order ? (order.Customer || "") : "",
                                    technicianId: technicianId,
                                    schDate: scheduleDate,
                                    qtyAssign: qty,
                                    deviceGroupId: selectedDeviceGroup,
                                    areaId: selectedAreaId,
                                    targetStatus: selectedStatus,
                                    insDeviceTypeId: getInsDeviceTypeId(assignModalState.joType, order),
                                    assignRemark: assignRemark
                                },
                                function (result) {
                                    var isSuccess = (result.Result || "").toUpperCase() === "SUCCESS";
                                    if (!isSuccess) {
                                        setAssignSubmitLoading(false);
                                        var failedMessage = result.Message || "Gagal menyimpan assignment.";
                                        if ((failedMessage || "").toLowerCase().indexOf("itid") >= 0) {
                                            window.alert("ITID not exist");
                                        }
                                        if ((failedMessage || "").toLowerCase().indexOf("catatan pindah") >= 0 && order) {
                                            order._requiresTransferNote = true;
                                            assignModalState.selectedOrder = order;
                                            renderOrderInfo();
                                        }
                                        setFeedback(failedMessage, true, false);
                                        showAssignToast(failedMessage, true);
                                        return;
                                    }

                                    var successMessage = result.Message || "Assignment berhasil disimpan.";
                                    setFeedback(successMessage, false, true);
                                    showAssignToast(successMessage, false);

                                    var techIds = [technicianId];
                                    var sourceTechId = (result && result.PreviousTechnicianId) || getOrderAssignedTechnicianId(order);
                                    var previousSchDate = (result && result.PreviousSchDate) || getOrderAssignedSchDate(order);
                                    if (sourceTechId && normalizeTechId(sourceTechId) !== normalizeTechId(technicianId)) {
                                        techIds.push(sourceTechId);
                                    }
                                    refreshAfterAssignChange({
                                        scheduleDate: (result && result.SchDate) || scheduleDate,
                                        technicianIds: techIds,
                                        previousSchDate: previousSchDate,
                                        previousTechnicianId: sourceTechId,
                                        primaryCell: activeCell
                                    }, function () {
                                        setAssignSubmitLoading(false);
                                        closeAssignModal();
                                    });
                                },
                                function (errorMessage) {
                                    setAssignSubmitLoading(false);
                                    setFeedback("Gagal menyimpan assignment. " + (errorMessage || ""), true, false);
                                    showAssignToast("Gagal menyimpan assignment.", true);
                                });
                        }

                        if (!isAvailableStatus || !order) {
                            runMainAssignSave("");
                            return;
                        }

                        enrichSelectedOrderAssignContext(order, "main", function (enriched) {
                            assignModalState.selectedOrder = enriched;
                            renderOrderInfo();
                            var assignRemark = "";
                            if (requiresTransferNoteForOrder(enriched, technicianId)) {
                                assignRemark = getDashboardNoteValue("assignTransferNoteInput");
                                if (!assignRemark) {
                                    setFeedback("Catatan pindah IT Support wajib diisi.", true, false);
                                    var transferInput = document.getElementById("assignTransferNoteInput");
                                    if (transferInput) {
                                        transferInput.focus();
                                    }
                                    return;
                                }
                            }
                            runMainAssignSave(assignRemark);
                        });
                    });
                }
                if (administrationBtn) {
                    administrationBtn.addEventListener("click", function () {
                        assignModalState.submitAction = "administration";
                        if (assignModalState.isSaving) {
                            return;
                        }

                        var selectedStatus = assignModalState.administrationTargetStatus === "UD" ? "UD" : "AD";
                        var isAvailableStatus = selectedStatus === "AV";
                        var selectedDeviceGroup = normalizeJoTypeForApi(assignModalState.joType) === "maintenance"
                            ? "GPS"
                            : normalizeDeviceGroup(assignModalState.deviceGroupId);
                        var order = getSelectedOrder();
                        if (isAvailableStatus && !order) {
                            setFeedback("Belum ada JO yang dapat dipilih.", true, false);
                            return;
                        }

                        var activeCell = assignModalState.activeCell;
                        if (!requireValidItId(activeCell, true)) {
                            return;
                        }
                        var technicianId = activeCell ? (activeCell.getAttribute("data-tech-id") || "") : "";
                        var scheduleDate = activeCell ? (activeCell.getAttribute("data-date") || "") : "";
                        if (!technicianId) {
                            setFeedback("IT Support tidak valid.", true, false);
                            return;
                        }
                        if (!scheduleDate) {
                            setFeedback("Tanggal schedule tidak valid.", true, false);
                            return;
                        }

                        var qty = 0;
                        var selectedAreaId = "";
                        var fallbackDisplay = selectedStatus;
                        if (isAvailableStatus) {
                            if (!validateAssignUnitInput(true)) {
                                return;
                            }
                            selectedAreaId = order ? normalizeAreaId(order.DefaultAreaId || "") : "";
                            qty = 1;
                            fallbackDisplay = "1";
                        }

                        setAssignSubmitLoading(true);
                        setFeedback("Menyimpan assignment...", false, false);

                        callAssignPageMethod(
                            "SaveAssignJob",
                            {
                                assignId: "",
                                jobId: order ? (order.JobID || "") : "",
                                custId: order ? (order.Customer || "") : "",
                                technicianId: technicianId,
                                schDate: scheduleDate,
                                qtyAssign: qty,
                                deviceGroupId: selectedDeviceGroup,
                                areaId: selectedAreaId,
                                targetStatus: selectedStatus,
                                insDeviceTypeId: getInsDeviceTypeId(assignModalState.joType, order)
                            },
                            function (result) {
                                var isSuccess = (result.Result || "").toUpperCase() === "SUCCESS";
                                if (!isSuccess) {
                                    setAssignSubmitLoading(false);
                                    var failedMessage = result.Message || "Gagal menyimpan assignment.";
                                    setFeedback(failedMessage, true, false);
                                    showAssignToast(failedMessage, true);
                                    return;
                                }

                                var successMessage = result.Message || "Assignment berhasil disimpan.";
                                setFeedback(successMessage, false, true);
                                showAssignToast(successMessage, false);

                                refreshAfterAssignChange({
                                    scheduleDate: (result && result.SchDate) || scheduleDate,
                                    technicianIds: [technicianId],
                                    previousSchDate: (result && result.PreviousSchDate) || "",
                                    previousTechnicianId: (result && result.PreviousTechnicianId) || "",
                                    primaryCell: activeCell
                                }, function () {
                                    setAssignSubmitLoading(false);
                                    closeAssignModal();
                                });
                            },
                            function (errorMessage) {
                                setAssignSubmitLoading(false);
                                setFeedback("Gagal menyimpan assignment. " + (errorMessage || ""), true, false);
                                showAssignToast("Gagal menyimpan assignment.", true);
                            });
                    });
                }
            }

            function bindAssignEvents() {
                bindAssignDocumentEvents();
                bindAssignModalControls();
            }

            function resetAssignInteractionAfterPostback() {
                assignModalState.activeCell = null;
                reportModalState.activeCell = null;
                closeAssignModal();
                closeCreateJoModal();
                closeStatusOnlyModal();
                closeCompletedReportModal();
                closeJoInfoModal();
                closeTechStockModal();
                closeTechDeviceDetailModal();
                syncDayTotalButtonLabels();
                syncCreateJoButtonVisibility();
                if (typeof $ !== "undefined") {
                    $("#overlay").stop(true, true).hide();
                }
            }

            window.switchAssignTab = function (tab) {
                var normalized = normalizeTab(tab);
                var tabField = document.getElementById("<%= hfActiveTab.ClientID %>");
                if (tabField) {
                    tabField.value = normalized;
                }
                setActiveTabUI(normalized);
                showOverlay();
                __doPostBack("<%= btnTabRefresh.UniqueID %>", "");
            };

            window.openSummaryDetail = function (status) {
                var statusValue = (status || "all").toLowerCase();
                if (statusValue !== "open" && statusValue !== "close" && statusValue !== "scheduled") {
                    statusValue = "all";
                }

                var statusField = document.getElementById("<%= hfDetailStatus.ClientID %>");
                var jobTypeField = document.getElementById("<%= hfDetailJobType.ClientID %>");
                var metricField = document.getElementById("<%= hfDetailMetric.ClientID %>");
                if (statusField) {
                    statusField.value = statusValue;
                }
                if (jobTypeField) {
                    jobTypeField.value = "all";
                }
                if (metricField) {
                    metricField.value = "jo";
                }
                showOverlay();
                __doPostBack("<%= btnOpenDetail.UniqueID %>", "");
                return false;
            };

            window.openSummaryDetailSplit = function (status, jobType, metric) {
                var statusValue = (status || "all").toLowerCase();
                if (statusValue !== "open" && statusValue !== "close" && statusValue !== "scheduled") {
                    statusValue = "all";
                }

                var jobTypeValue = (jobType || "all").toLowerCase();
                if (jobTypeValue !== "new_installation" && jobTypeValue !== "maintenance") {
                    jobTypeValue = "all";
                }

                var metricValue = (metric || "jo").toLowerCase() === "unit" ? "unit" : "jo";

                var statusField = document.getElementById("<%= hfDetailStatus.ClientID %>");
                var jobTypeField = document.getElementById("<%= hfDetailJobType.ClientID %>");
                var metricField = document.getElementById("<%= hfDetailMetric.ClientID %>");
                if (statusField) {
                    statusField.value = statusValue;
                }
                if (jobTypeField) {
                    jobTypeField.value = jobTypeValue;
                }
                if (metricField) {
                    metricField.value = metricValue;
                }

                showOverlay();
                __doPostBack("<%= btnOpenDetail.UniqueID %>", "");
                return false;
            };

            window.exportDetailJoToExcel = function () {
                var statusField = document.getElementById("<%= hfDetailStatus.ClientID %>");
                var jobTypeField = document.getElementById("<%= hfDetailJobType.ClientID %>");
                var metricField = document.getElementById("<%= hfDetailMetric.ClientID %>");
                var statusValue = statusField ? statusField.value : "all";
                var jobTypeValue = jobTypeField ? jobTypeField.value : "all";
                var metricValue = metricField ? metricField.value : "jo";
                var exportUrl = getAssignPageUrl() + "?action=export_detail"
                    + "&status=" + encodeURIComponent(statusValue)
                    + "&detailJobType=" + encodeURIComponent(jobTypeValue)
                    + "&detailMetric=" + encodeURIComponent(metricValue);
                window.open(exportUrl, "_blank");
                return false;
            };

            function initAssignDashboard() {
                var tabField = document.getElementById("<%= hfActiveTab.ClientID %>");
                setActiveTabUI(normalizeTab(tabField ? tabField.value : "teknisi"));
            }

            var assignPerfChartState = {
                dailyInstance: null,
                monthlyInstance: null,
                resizeBound: false,
                isLoading: false,
                technicians: [],
                selectedTechId: "",
                dailyRows: [],
                dailyTechName: "",
                dailyYear: 0,
                dailyMonth: 0,
                dailyMaxEndDay: 0,
                dailyWindowStart: 0,
                dailyWindowEnd: 0
            };

            function getPerfPeriodeString() {
                var periodeField = document.getElementById("<%= txtPeriode.ClientID %>");
                var raw = periodeField ? (periodeField.value || "").trim() : "";
                var match = raw.match(/(\d{4})[-/](\d{1,2})/);
                if (match) {
                    var month = parseInt(match[2], 10);
                    return match[1] + "-" + (month < 10 ? "0" + month : String(month));
                }

                var now = new Date();
                var currentMonth = now.getMonth() + 1;
                return now.getFullYear() + "-" + (currentMonth < 10 ? "0" + currentMonth : String(currentMonth));
            }

            function getPerfActiveTab() {
                var tabField = document.getElementById("<%= hfActiveTab.ClientID %>");
                return normalizeTab(tabField ? tabField.value : "teknisi");
            }

            function getPerfSelectedPeriodeParts() {
                var periode = getPerfPeriodeString();
                var parts = periode.split("-");
                return {
                    year: parseInt(parts[0], 10) || new Date().getFullYear(),
                    month: parseInt(parts[1], 10) || (new Date().getMonth() + 1),
                    value: periode
                };
            }

            function formatPerfMonthLabelFromValue(periodeMonth) {
                if (!periodeMonth) {
                    return "-";
                }
                var parts = (periodeMonth || "").split("-");
                if (parts.length !== 2) {
                    return periodeMonth;
                }
                return formatPerfMonthLabel(parseInt(parts[0], 10), parseInt(parts[1], 10));
            }

            function formatPerfMonthLabel(year, month) {
                var monthNames = ["Jan", "Feb", "Mar", "Apr", "Mei", "Jun", "Jul", "Agu", "Sep", "Okt", "Nov", "Des"];
                var safeMonth = Math.max(1, Math.min(12, month));
                return monthNames[safeMonth - 1] + " " + year;
            }

            function setPerfChartNote(message) {
                var note = document.getElementById("assignPerfChartNote");
                if (note) {
                    note.textContent = message || "";
                }
            }

            function populateAssignPerfTechSelect(technicians, selectedTechId) {
                var select = document.getElementById("assignPerfTechSelect");
                if (!select) {
                    return "";
                }

                var techList = technicians || [];
                var currentValue = selectedTechId || select.value || (techList[0] ? techList[0].TechnicianID : "");
                if (!techList.length) {
                    select.innerHTML = "<option value=\"\">Tidak ada IT Support</option>";
                    select.value = "";
                    return "";
                }

                var html = "";
                for (var i = 0; i < techList.length; i++) {
                    var tech = techList[i] || {};
                    var techId = tech.TechnicianID || tech.technicianID || "";
                    var techName = tech.TechnicianName || tech.technicianName || techId || "-";
                    html += "<option value=\"" + techId + "\">" + techName + "</option>";
                }
                select.innerHTML = html;

                var hasCurrent = false;
                for (var j = 0; j < techList.length; j++) {
                    var row = techList[j] || {};
                    var rowId = row.TechnicianID || row.technicianID || "";
                    if (rowId === currentValue) {
                        hasCurrent = true;
                        break;
                    }
                }

                var firstId = techList[0].TechnicianID || techList[0].technicianID || "";
                select.value = hasCurrent ? currentValue : firstId;
                return select.value;
            }

            function getSelectedPerfTechnician() {
                var select = document.getElementById("assignPerfTechSelect");
                var selectedId = select ? (select.value || "").trim() : "";
                var techList = assignPerfChartState.technicians || [];
                for (var i = 0; i < techList.length; i++) {
                    var tech = techList[i] || {};
                    var techId = tech.TechnicianID || tech.technicianID || "";
                    if (techId === selectedId) {
                        return {
                            id: techId,
                            name: tech.TechnicianName || tech.technicianName || techId
                        };
                    }
                }
                return {
                    id: selectedId,
                    name: selectedId || "-"
                };
            }

            function getPerfDailyMaxEndDay(year, month) {
                var daysInMonth = new Date(year, month, 0).getDate();
                var today = new Date();
                if (today.getFullYear() === year && (today.getMonth() + 1) === month) {
                    return today.getDate();
                }
                return daysInMonth;
            }

            function getDefaultPerfDailyWindow(year, month) {
                var maxEndDay = getPerfDailyMaxEndDay(year, month);
                return {
                    startDay: Math.max(1, maxEndDay - 6),
                    endDay: maxEndDay,
                    maxEndDay: maxEndDay
                };
            }

            function filterDailyRowsForRange(rows, startDay, endDay) {
                var filtered = [];
                var list = rows || [];

                for (var i = 0; i < list.length; i++) {
                    var row = list[i] || {};
                    var dayNo = parseInt(row.DayNo, 10);
                    if (dayNo >= startDay && dayNo <= endDay) {
                        filtered.push(row);
                    }
                }

                filtered.sort(function (a, b) {
                    return (parseInt(a.DayNo, 10) || 0) - (parseInt(b.DayNo, 10) || 0);
                });

                return filtered;
            }

            function updatePerfDailyWeekNavButtons() {
                var prevBtn = document.getElementById("assignPerfDailyWeekPrev");
                var nextBtn = document.getElementById("assignPerfDailyWeekNext");
                var hasData = (assignPerfChartState.dailyRows || []).length > 0;
                var canPrev = hasData && assignPerfChartState.dailyWindowStart > 1;
                var canNext = hasData
                    && assignPerfChartState.dailyWindowEnd < assignPerfChartState.dailyMaxEndDay;

                if (prevBtn) {
                    prevBtn.disabled = !canPrev;
                }
                if (nextBtn) {
                    nextBtn.disabled = !canNext;
                }
            }

            function setPerfDailyWeekWindow(startDay, endDay, year, month) {
                var defaults = getDefaultPerfDailyWindow(year, month);
                assignPerfChartState.dailyYear = year;
                assignPerfChartState.dailyMonth = month;
                assignPerfChartState.dailyMaxEndDay = defaults.maxEndDay;
                assignPerfChartState.dailyWindowStart = Math.max(1, startDay);
                assignPerfChartState.dailyWindowEnd = Math.min(defaults.maxEndDay, endDay);
                if (assignPerfChartState.dailyWindowEnd < assignPerfChartState.dailyWindowStart) {
                    assignPerfChartState.dailyWindowEnd = assignPerfChartState.dailyWindowStart;
                }
                updatePerfDailyWeekNavButtons();
            }

            function resetPerfDailyWeekWindow(year, month) {
                var defaults = getDefaultPerfDailyWindow(year, month);
                setPerfDailyWeekWindow(defaults.startDay, defaults.endDay, year, month);
            }

            function renderCachedPerfDailyChart() {
                var rows = filterDailyRowsForRange(
                    assignPerfChartState.dailyRows,
                    assignPerfChartState.dailyWindowStart,
                    assignPerfChartState.dailyWindowEnd);
                renderAssignPerfDailyChart(
                    rows,
                    assignPerfChartState.dailyTechName,
                    assignPerfChartState.dailyYear,
                    assignPerfChartState.dailyMonth,
                    assignPerfChartState.dailyWindowStart,
                    assignPerfChartState.dailyWindowEnd);
                updatePerfDailyWeekNavButtons();
            }

            function shiftPerfDailyWeek(direction) {
                if (!(assignPerfChartState.dailyRows || []).length) {
                    return;
                }

                var startDay = assignPerfChartState.dailyWindowStart;
                var endDay = assignPerfChartState.dailyWindowEnd;
                var maxEndDay = assignPerfChartState.dailyMaxEndDay;
                var newStart = startDay;
                var newEnd = endDay;

                if (direction < 0) {
                    if (startDay <= 1) {
                        return;
                    }
                    newEnd = startDay - 1;
                    newStart = Math.max(1, newEnd - 6);
                } else if (direction > 0) {
                    if (endDay >= maxEndDay) {
                        return;
                    }
                    newStart = endDay + 1;
                    newEnd = Math.min(maxEndDay, newStart + 6);
                } else {
                    return;
                }

                assignPerfChartState.dailyWindowStart = newStart;
                assignPerfChartState.dailyWindowEnd = newEnd;
                renderCachedPerfDailyChart();
            }

            function disposeAssignPerfChart(instanceKey) {
                if (assignPerfChartState[instanceKey]) {
                    try {
                        assignPerfChartState[instanceKey].dispose();
                    } catch (ex) {
                    }
                    assignPerfChartState[instanceKey] = null;
                }
            }

            function renderAssignPerfDailyChart(rows, techName, year, month, startDay, endDay) {
                var chartDom = document.getElementById("assignPerfDailyChart");
                var subtitle = document.getElementById("assignPerfDailySubtitle");
                if (!chartDom || typeof echarts === "undefined") {
                    return;
                }

                var labels = [];
                var joClose = [];
                var unitClose = [];
                var joAssign = [];
                var dataRows = rows || [];

                for (var i = 0; i < dataRows.length; i++) {
                    var row = dataRows[i] || {};
                    labels.push((row.DayNo || "") + "/" + month);
                    joClose.push(parseInt(row.JoCloseTotal, 10) || 0);
                    unitClose.push(parseInt(row.UnitCloseTotal, 10) || 0);
                    joAssign.push(parseInt(row.JoAssignTotal, 10) || 0);
                }

                if (subtitle) {
                    subtitle.textContent = "Hari " + startDay + "-" + endDay + " | " + formatPerfMonthLabel(year, month) + " | " + (techName || "-");
                }

                disposeAssignPerfChart("dailyInstance");
                assignPerfChartState.dailyInstance = echarts.init(chartDom);
                assignPerfChartState.dailyInstance.setOption({
                    tooltip: { trigger: "axis", confine: true },
                    legend: { data: ["JO Close", "Unit Close", "JO Assign"], bottom: 0, textStyle: { fontSize: 11 } },
                    grid: { left: "3%", right: "3%", bottom: "16%", top: "8%", containLabel: true },
                    xAxis: {
                        type: "category",
                        data: labels,
                        axisLabel: { fontSize: 10, color: "#6f6f68" }
                    },
                    yAxis: {
                        type: "value",
                        min: 0,
                        axisLabel: { fontSize: 10, color: "#6f6f68" },
                        splitLine: { lineStyle: { color: "#eceae6" } }
                    },
                    series: [
                        { name: "JO Close", type: "bar", data: joClose, itemStyle: { color: "#16a34a" }, barMaxWidth: 18 },
                        { name: "Unit Close", type: "bar", data: unitClose, itemStyle: { color: "#0a5c48" }, barMaxWidth: 18 },
                        { name: "JO Assign", type: "line", smooth: true, data: joAssign, itemStyle: { color: "#2563eb" }, lineStyle: { width: 2 } }
                    ]
                });
                assignPerfChartState.dailyInstance.resize();
            }

            function renderAssignPerfMonthlyChart(rows, techName) {
                var chartDom = document.getElementById("assignPerfMonthlyChart");
                var subtitle = document.getElementById("assignPerfMonthlySubtitle");
                if (!chartDom || typeof echarts === "undefined") {
                    return;
                }

                var labels = [];
                var joClose = [];
                var unitClose = [];
                var joAssign = [];
                var dataRows = rows || [];

                for (var i = 0; i < dataRows.length; i++) {
                    var row = dataRows[i] || {};
                    labels.push(formatPerfMonthLabelFromValue(row.PeriodeMonth));
                    joClose.push(parseInt(row.JoCloseTotal, 10) || 0);
                    unitClose.push(parseInt(row.UnitCloseTotal, 10) || 0);
                    joAssign.push(parseInt(row.JoAssignTotal, 10) || 0);
                }

                if (subtitle) {
                    subtitle.textContent = "6 bulan terakhir | " + (techName || "-");
                }

                disposeAssignPerfChart("monthlyInstance");
                assignPerfChartState.monthlyInstance = echarts.init(chartDom);
                assignPerfChartState.monthlyInstance.setOption({
                    tooltip: { trigger: "axis", confine: true },
                    legend: { data: ["JO Close", "Unit Close", "JO Assign"], bottom: 0, textStyle: { fontSize: 11 } },
                    grid: { left: "3%", right: "3%", bottom: "16%", top: "8%", containLabel: true },
                    xAxis: {
                        type: "category",
                        data: labels,
                        axisLabel: { fontSize: 10, color: "#6f6f68", rotate: 20 }
                    },
                    yAxis: {
                        type: "value",
                        min: 0,
                        axisLabel: { fontSize: 10, color: "#6f6f68" },
                        splitLine: { lineStyle: { color: "#eceae6" } }
                    },
                    series: [
                        { name: "JO Close", type: "line", smooth: true, data: joClose, itemStyle: { color: "#16a34a" }, lineStyle: { width: 2 } },
                        { name: "Unit Close", type: "line", smooth: true, data: unitClose, itemStyle: { color: "#0a5c48" }, lineStyle: { width: 2 } },
                        { name: "JO Assign", type: "bar", data: joAssign, itemStyle: { color: "#2563eb" }, barMaxWidth: 22 }
                    ]
                });
                assignPerfChartState.monthlyInstance.resize();
            }

            function renderEmptyAssignPerfCharts(message, techName, year, month) {
                assignPerfChartState.dailyRows = [];
                assignPerfChartState.dailyTechName = techName || "-";
                resetPerfDailyWeekWindow(year, month);
                renderAssignPerfDailyChart([], techName, year, month, 0, 0);
                renderAssignPerfMonthlyChart([], techName);
                updatePerfDailyWeekNavButtons();
                setPerfChartNote(message || "Belum ada data grafik kinerja.");
            }

            function updateAssignPerfChartTitle(activeTab) {
                var title = document.getElementById("lblPerfChartTabTitle");
                if (title) {
                    title.textContent = activeTab === "itsupport" ? "IT Support" : "Teknisi";
                }
            }

            function loadPerfChartDataFromApi(technicianId, callback) {
                var periode = getPerfPeriodeString();
                callAssignPageMethod(
                    "LoadPerfChartData",
                    {
                        periode: periode,
                        technicianId: technicianId || ""
                    },
                    function (result) {
                        var isSuccess = (result && result.Result ? result.Result : "").toUpperCase() === "SUCCESS";
                        if (typeof callback === "function") {
                            callback(isSuccess ? result : null, result);
                        }
                    },
                    function (errorMessage) {
                        if (typeof callback === "function") {
                            callback(null, { Message: errorMessage || "Gagal memuat data grafik." });
                        }
                    });
            }

            function applyAssignPerfChartResult(chartResult, rawResult, technician, periodeParts) {
                if (!chartResult) {
                    renderEmptyAssignPerfCharts(
                        (rawResult && rawResult.Message) || "Gagal memuat data grafik.",
                        technician.name,
                        periodeParts.year,
                        periodeParts.month);
                    return;
                }

                var techName = chartResult.TechnicianName || technician.name || technician.id;
                assignPerfChartState.dailyRows = chartResult.DailyRows || [];
                assignPerfChartState.dailyTechName = techName;
                resetPerfDailyWeekWindow(periodeParts.year, periodeParts.month);
                renderCachedPerfDailyChart();
                renderAssignPerfMonthlyChart(chartResult.MonthlyRows || [], techName);
                setPerfChartNote("Data grafik mengikuti periode " + periodeParts.value + " dan filter area group. Gunakan panah untuk melihat minggu sebelumnya/berikutnya.");
            }

            function handlePerfTechSelectChange() {
                var technician = getSelectedPerfTechnician();
                var periodeParts = getPerfSelectedPeriodeParts();
                assignPerfChartState.selectedTechId = technician.id || "";

                if (!technician.id) {
                    renderEmptyAssignPerfCharts("Pilih IT Support terlebih dahulu.", "-", periodeParts.year, periodeParts.month);
                    return;
                }

                if (assignPerfChartState.isLoading) {
                    return;
                }

                assignPerfChartState.isLoading = true;
                setPerfChartNote("Memuat data grafik kinerja...");
                loadPerfChartDataFromApi(technician.id, function (chartResult, rawResult) {
                    assignPerfChartState.isLoading = false;
                    applyAssignPerfChartResult(chartResult, rawResult, technician, periodeParts);
                });
            }

            function loadPerfTechniciansFromApi(callback) {
                callAssignPageMethod(
                    "LoadPerfTechnicianList",
                    { periode: getPerfPeriodeString() },
                    function (result) {
                        var isSuccess = (result && result.Result ? result.Result : "").toUpperCase() === "SUCCESS";
                        assignPerfChartState.technicians = isSuccess && result.Rows ? result.Rows : [];
                        if (typeof callback === "function") {
                            callback(assignPerfChartState.technicians, result);
                        }
                    },
                    function (errorMessage) {
                        assignPerfChartState.technicians = [];
                        if (typeof callback === "function") {
                            callback([], { Message: errorMessage || "Gagal memuat IT Support." });
                        }
                    });
            }

            function renderAssignPerfCharts() {
                if (typeof echarts === "undefined") {
                    return;
                }

                var activeTab = getPerfActiveTab();
                var periodeParts = getPerfSelectedPeriodeParts();
                updateAssignPerfChartTitle(activeTab);

                if (assignPerfChartState.isLoading) {
                    return;
                }

                assignPerfChartState.isLoading = true;
                setPerfChartNote("Memuat data grafik kinerja...");

                loadPerfTechniciansFromApi(function (technicians, listResult) {
                    var select = document.getElementById("assignPerfTechSelect");
                    var preferredTechId = assignPerfChartState.selectedTechId
                        || (select ? (select.value || "").trim() : "");
                    var selectedTechId = populateAssignPerfTechSelect(technicians, preferredTechId);
                    var technician = getSelectedPerfTechnician();
                    assignPerfChartState.selectedTechId = selectedTechId;

                    if (!selectedTechId) {
                        assignPerfChartState.isLoading = false;
                        renderEmptyAssignPerfCharts((listResult && listResult.Message) || "Belum ada IT Support pada filter ini.", "-", periodeParts.year, periodeParts.month);
                        return;
                    }

                    loadPerfChartDataFromApi(selectedTechId, function (chartResult, rawResult) {
                        assignPerfChartState.isLoading = false;
                        applyAssignPerfChartResult(chartResult, rawResult, technician, periodeParts);
                    });
                });
            }

            function bindAssignPerfChartEvents() {
                if (!window.__assignPerfChartDocBound) {
                    document.addEventListener("change", function (evt) {
                        var target = evt.target || evt.srcElement;
                        if (!target || target.id !== "assignPerfTechSelect") {
                            return;
                        }
                        handlePerfTechSelectChange();
                    });
                    document.addEventListener("click", function (evt) {
                        var target = evt.target || evt.srcElement;
                        if (!target) {
                            return;
                        }
                        if (target.id === "assignPerfDailyWeekPrev") {
                            shiftPerfDailyWeek(-1);
                            return;
                        }
                        if (target.id === "assignPerfDailyWeekNext") {
                            shiftPerfDailyWeek(1);
                        }
                    });
                    window.__assignPerfChartDocBound = true;
                }

                if (!assignPerfChartState.resizeBound) {
                    window.addEventListener("resize", function () {
                        if (assignPerfChartState.dailyInstance) {
                            assignPerfChartState.dailyInstance.resize();
                        }
                        if (assignPerfChartState.monthlyInstance) {
                            assignPerfChartState.monthlyInstance.resize();
                        }
                    });
                    assignPerfChartState.resizeBound = true;
                }
            }

            function initAssignPerfCharts() {
                assignPerfChartState.isLoading = false;
                bindAssignPerfChartEvents();
                renderAssignPerfCharts();
            }

            function initPeriodePicker() {
                if (typeof $ === "undefined" || !$.fn || !$.fn.datepicker) {
                    return;
                }

                var picker = $("#<%= txtPeriode.ClientID %>");
                if (!picker.length) {
                    return;
                }

                try {
                    picker.datepicker("destroy");
                } catch (ex) {
                }

                picker.datepicker({
                    format: "MM yyyy",
                    language: "id",
                    minViewMode: 1,
                    autoclose: true,
                    orientation: "bottom auto"
                });
            }

            var detailJoSortState = {
                colIndex: 4,
                direction: "asc"
            };

            function getDetailJoSortType(colIndex) {
                var table = document.querySelector("#modal-detail-jo .detail-jo-table");
                if (!table) {
                    return "text";
                }

                var th = table.querySelector('thead th[data-sort-col="' + colIndex + '"]');
                return th ? (th.getAttribute("data-sort-type") || "text") : "text";
            }

            function parseDetailJoSortValue(text, sortType) {
                var value = (text || "").trim();
                if (!value || value === "-") {
                    if (sortType === "number") {
                        return -1;
                    }
                    if (sortType === "date") {
                        return 0;
                    }
                    return "";
                }

                if (sortType === "number") {
                    var num = parseFloat(value.replace(/[^0-9.-]/g, ""));
                    return isNaN(num) ? -1 : num;
                }

                if (sortType === "date") {
                    var ts = Date.parse(value);
                    return isNaN(ts) ? 0 : ts;
                }

                return value.toLowerCase();
            }

            function compareDetailJoValues(a, b, sortType, direction) {
                var multiplier = direction === "desc" ? -1 : 1;
                if (sortType === "number" || sortType === "date") {
                    if (a < b) {
                        return -1 * multiplier;
                    }
                    if (a > b) {
                        return 1 * multiplier;
                    }
                    return 0;
                }

                if (typeof a.localeCompare === "function") {
                    return a.localeCompare(b, "id", { numeric: true, sensitivity: "base" }) * multiplier;
                }

                if (a < b) {
                    return -1 * multiplier;
                }
                if (a > b) {
                    return 1 * multiplier;
                }
                return 0;
            }

            function updateDetailJoSortHeaders(activeCol, direction) {
                var headers = document.querySelectorAll("#modal-detail-jo .detail-jo-sortable");
                for (var i = 0; i < headers.length; i++) {
                    var th = headers[i];
                    th.classList.remove("is-sorted", "is-sorted-asc", "is-sorted-desc");
                    if (parseInt(th.getAttribute("data-sort-col"), 10) === activeCol) {
                        th.classList.add("is-sorted", direction === "desc" ? "is-sorted-desc" : "is-sorted-asc");
                    }
                }
            }

            function updateDetailJoRowNumbers() {
                var tableBody = document.getElementById("detailJoTableBody");
                if (!tableBody) {
                    return;
                }

                var rows = tableBody.querySelectorAll("tr.detail-jo-row");
                var rowNo = 1;
                for (var i = 0; i < rows.length; i++) {
                    if (rows[i].style.display === "none") {
                        continue;
                    }
                    if (rows[i].cells[0]) {
                        rows[i].cells[0].textContent = rowNo.toString();
                        rowNo++;
                    }
                }
            }

            function sortDetailJoTable(colIndex, direction, forceDirection) {
                var tableBody = document.getElementById("detailJoTableBody");
                if (!tableBody) {
                    return;
                }

                var sortType = getDetailJoSortType(colIndex);
                if (forceDirection) {
                    direction = direction || "asc";
                } else if (detailJoSortState.colIndex === colIndex) {
                    direction = detailJoSortState.direction === "asc" ? "desc" : "asc";
                } else {
                    direction = "asc";
                }

                var rows = Array.prototype.slice.call(tableBody.querySelectorAll("tr.detail-jo-row"));
                if (!rows.length) {
                    return;
                }

                rows.sort(function (rowA, rowB) {
                    var cellA = rowA.cells[colIndex];
                    var cellB = rowB.cells[colIndex];
                    var valA = parseDetailJoSortValue(cellA ? cellA.textContent : "", sortType);
                    var valB = parseDetailJoSortValue(cellB ? cellB.textContent : "", sortType);
                    return compareDetailJoValues(valA, valB, sortType, direction);
                });

                for (var i = 0; i < rows.length; i++) {
                    tableBody.appendChild(rows[i]);
                }

                detailJoSortState.colIndex = colIndex;
                detailJoSortState.direction = direction;
                updateDetailJoSortHeaders(colIndex, direction);
                updateDetailJoRowNumbers();
            }

            function initDetailJoSort() {
                var table = document.querySelector("#modal-detail-jo .detail-jo-table");
                if (!table) {
                    return;
                }

                if (table.getAttribute("data-sort-bound") !== "1") {
                    var headers = table.querySelectorAll(".detail-jo-sortable");
                    for (var i = 0; i < headers.length; i++) {
                        (function (th) {
                            th.addEventListener("click", function () {
                                var colIndex = parseInt(th.getAttribute("data-sort-col"), 10);
                                if (isNaN(colIndex)) {
                                    return;
                                }
                                sortDetailJoTable(colIndex);
                                applyDetailJoSearch();
                            });
                        })(headers[i]);
                    }
                    table.setAttribute("data-sort-bound", "1");
                }

                var rows = document.querySelectorAll("#detailJoTableBody tr.detail-jo-row");
                if (rows.length > 0) {
                    sortDetailJoTable(detailJoSortState.colIndex, detailJoSortState.direction, true);
                }
            }

            function applyDetailJoSearch() {
                var searchInput = document.getElementById("detailJoSearchInput");
                var tableBody = document.getElementById("detailJoTableBody");
                if (!searchInput || !tableBody) {
                    return;
                }

                var keyword = (searchInput.value || "").toLowerCase().trim();
                var rows = tableBody.querySelectorAll("tr.detail-jo-row");
                var visibleCount = 0;
                for (var i = 0; i < rows.length; i++) {
                    var row = rows[i];
                    var rowText = (row.textContent || "").toLowerCase();
                    var isVisible = !keyword || rowText.indexOf(keyword) !== -1;
                    row.style.display = isVisible ? "" : "none";
                    if (isVisible) {
                        visibleCount++;
                    }
                }

                var emptyRow = document.getElementById("detailJoSearchEmptyRow");
                if (keyword && rows.length > 0 && visibleCount === 0) {
                    if (!emptyRow) {
                        emptyRow = document.createElement("tr");
                        emptyRow.id = "detailJoSearchEmptyRow";
                        emptyRow.className = "detail-jo-search-empty";
                        emptyRow.innerHTML = "<td colspan=\"12\">Data tidak ditemukan untuk pencarian ini.</td>";
                        tableBody.appendChild(emptyRow);
                    }
                } else if (emptyRow && emptyRow.parentNode) {
                    emptyRow.parentNode.removeChild(emptyRow);
                }

                updateDetailJoRowNumbers();
            }

            function initDetailJoSearch() {
                var searchInput = document.getElementById("detailJoSearchInput");
                if (!searchInput) {
                    return;
                }

                if (!searchInput.getAttribute("data-bound")) {
                    searchInput.addEventListener("input", applyDetailJoSearch);
                    searchInput.setAttribute("data-bound", "1");
                }
                initDetailJoSort();
                applyDetailJoSearch();
            }

            function initAssignInteraction(fromPostBack) {
                bindAssignEvents();
                decorateAssignCells();
                initDetailJoSearch();
                initAssignPerfCharts();
                if (fromPostBack) {
                    resetAssignInteractionAfterPostback();
                }
            }

            document.addEventListener("DOMContentLoaded", function () {
                initAssignDashboard();
                initPeriodePicker();
                initAssignInteraction(false);
            });
            if (typeof (Sys) !== "undefined" && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                    initAssignDashboard();
                    initPeriodePicker();
                    initAssignInteraction(true);
                });
            }
        })();
    </script>
</asp:Content>
