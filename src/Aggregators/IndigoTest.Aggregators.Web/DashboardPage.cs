namespace IndigoTest.Aggregators.Web;

internal static class DashboardPage
{
    public static string Html =>
        """
        <!DOCTYPE html>
        <html lang="ru">
        <head>
            <meta charset="utf-8" />
            <meta name="viewport" content="width=device-width, initial-scale=1" />
            <title>IndigoTest Dashboard</title>
            <style>
                :root {
                    color-scheme: light;
                    --bg: #f4f1ea;
                    --surface: #fffdf8;
                    --surface-alt: #f0e9dc;
                    --line: #d8cdb8;
                    --text: #2c2418;
                    --muted: #756851;
                    --accent: #1f6f5f;
                    --accent-soft: #d9eee8;
                    --danger: #b54b4b;
                    --danger-soft: #f8e0e0;
                }

                * {
                    box-sizing: border-box;
                }

                body {
                    margin: 0;
                    font-family: "Segoe UI", Tahoma, sans-serif;
                    background:
                        radial-gradient(circle at top left, #efe2c8 0, transparent 28%),
                        linear-gradient(180deg, #f8f3e8 0%, var(--bg) 100%);
                    color: var(--text);
                }

                button {
                    font: inherit;
                }

                .page {
                    max-width: 1280px;
                    margin: 0 auto;
                    padding: 32px 20px 48px;
                }

                .hero {
                    display: grid;
                    gap: 12px;
                    margin-bottom: 24px;
                    padding: 24px;
                    background: rgba(255, 253, 248, 0.88);
                    border: 1px solid rgba(216, 205, 184, 0.8);
                    border-radius: 24px;
                    backdrop-filter: blur(10px);
                }

                .hero h1 {
                    margin: 0;
                    font-size: clamp(28px, 4vw, 42px);
                    line-height: 1.05;
                }

                .hero p {
                    margin: 0;
                    color: var(--muted);
                    max-width: 760px;
                }

                .hero-meta {
                    display: flex;
                    flex-wrap: wrap;
                    gap: 12px;
                    margin-top: 8px;
                    color: var(--muted);
                    font-size: 14px;
                }

                .grid {
                    display: grid;
                    gap: 16px;
                }

                .stats {
                    grid-template-columns: repeat(auto-fit, minmax(170px, 1fr));
                    margin-bottom: 24px;
                }

                .card {
                    background: var(--surface);
                    border: 1px solid var(--line);
                    border-radius: 20px;
                    padding: 18px;
                    box-shadow: 0 12px 32px rgba(68, 50, 18, 0.06);
                }

                .stat-title {
                    color: var(--muted);
                    font-size: 13px;
                    margin-bottom: 10px;
                    text-transform: uppercase;
                    letter-spacing: 0.08em;
                }

                .stat-value {
                    font-size: clamp(22px, 3vw, 32px);
                    font-weight: 700;
                }

                .panel {
                    display: grid;
                    gap: 12px;
                }

                .toolbar {
                    display: flex;
                    flex-wrap: wrap;
                    gap: 10px;
                    margin-bottom: 8px;
                }

                .panel-head {
                    display: flex;
                    justify-content: space-between;
                    align-items: center;
                    gap: 12px;
                }

                .panel h2 {
                    margin: 0;
                    font-size: 20px;
                }

                .badge {
                    display: inline-flex;
                    align-items: center;
                    gap: 8px;
                    padding: 8px 12px;
                    border-radius: 999px;
                    background: var(--accent-soft);
                    color: var(--accent);
                    font-size: 13px;
                    font-weight: 600;
                }

                .badge-danger {
                    background: var(--danger-soft);
                    color: var(--danger);
                }

                .tables {
                    grid-template-columns: minmax(0, 1fr);
                }

                table {
                    width: 100%;
                    border-collapse: collapse;
                    overflow: hidden;
                    border-radius: 16px;
                }

                thead {
                    background: var(--surface-alt);
                }

                th, td {
                    text-align: left;
                    padding: 12px 14px;
                    border-bottom: 1px solid var(--line);
                    font-size: 14px;
                    vertical-align: top;
                }

                tbody tr:last-child td {
                    border-bottom: none;
                }

                .empty {
                    color: var(--muted);
                    padding: 20px 0 4px;
                }

                .simulators {
                    grid-template-columns: repeat(auto-fit, minmax(320px, 1fr));
                }

                .simulator-card {
                    display: grid;
                    gap: 14px;
                }

                .simulator-top {
                    display: flex;
                    justify-content: space-between;
                    gap: 12px;
                    align-items: flex-start;
                }

                .simulator-name {
                    margin: 0;
                    font-size: 18px;
                }

                .simulator-kind {
                    color: var(--muted);
                    font-size: 13px;
                    margin-top: 4px;
                }

                .simulator-url {
                    color: var(--muted);
                    font-size: 12px;
                    word-break: break-all;
                }

                .simulator-state {
                    display: grid;
                    gap: 8px;
                    padding: 12px;
                    background: var(--surface-alt);
                    border-radius: 14px;
                }

                .simulator-state-line {
                    font-size: 13px;
                    color: var(--muted);
                }

                .simulator-metrics {
                    display: grid;
                    grid-template-columns: repeat(2, minmax(0, 1fr));
                    gap: 10px;
                }

                .metric-box {
                    padding: 12px;
                    border-radius: 14px;
                    border: 1px solid var(--line);
                    background: rgba(255, 255, 255, 0.6);
                }

                .metric-label {
                    font-size: 12px;
                    color: var(--muted);
                    margin-bottom: 6px;
                }

                .metric-value {
                    font-size: 18px;
                    font-weight: 700;
                }

                .actions {
                    display: flex;
                    flex-wrap: wrap;
                    gap: 8px;
                }

                .btn {
                    border: 1px solid transparent;
                    border-radius: 999px;
                    padding: 9px 12px;
                    cursor: pointer;
                    transition: transform 0.15s ease, opacity 0.15s ease;
                }

                .btn:hover {
                    transform: translateY(-1px);
                }

                .btn-primary {
                    background: var(--accent);
                    color: white;
                }

                .btn-soft {
                    background: var(--accent-soft);
                    color: var(--accent);
                }

                .btn-danger {
                    background: var(--danger-soft);
                    color: var(--danger);
                }

                .hint {
                    font-size: 12px;
                    color: var(--muted);
                }

                .action-status {
                    min-height: 18px;
                    font-size: 13px;
                    color: var(--muted);
                }

                .error {
                    color: var(--danger);
                    font-size: 13px;
                }

                @media (max-width: 720px) {
                    .page {
                        padding: 20px 14px 36px;
                    }

                    th, td {
                        padding: 10px;
                        font-size: 13px;
                    }
                }
            </style>
        </head>
        <body>
            <main class="page">
                <section class="hero">
                    <h1>IndigoTest Aggregator Dashboard</h1>
                    <p>
                        Простая живая форма для показа, что агрегатор принимает данные, пишет их в базу,
                        переживает сбои источников и позволяет вручную включать негативные сценарии.
                    </p>
                    <div class="hero-meta">
                        <span id="last-update">Обновление: -</span>
                        <span id="sources-count">Источники: -</span>
                    </div>
                </section>

                <section class="grid stats" id="stats"></section>

                <section class="card panel">
                    <div class="panel-head">
                        <h2>Быстрые сценарии</h2>
                        <span class="badge">Для ручной проверки устойчивости</span>
                    </div>
                    <div class="toolbar">
                        <button class="btn btn-primary" onclick="applyPresetToAll('duplicatesOn')">Включить дубликаты на всех</button>
                        <button class="btn btn-danger" onclick="applyPresetToAll('disconnectNext')">Разорвать все next</button>
                        <button class="btn btn-danger" onclick="applyPresetToAll('outageMinute')">Уронить всех на 1 минуту</button>
                        <button class="btn btn-soft" onclick="applyPresetToAll('silenceNext')">Всем тишину 5 сек</button>
                        <button class="btn btn-soft" onclick="applyPresetToAll('reset')">Сбросить все</button>
                    </div>
                    <div class="action-status" id="action-status"></div>
                </section>

                <section class="grid simulators" id="simulators"></section>

                <section class="grid tables">
                    <article class="card panel">
                        <div class="panel-head">
                            <h2>Последние записанные тики</h2>
                            <span class="badge" id="write-badge">Записано: 0</span>
                        </div>
                        <table>
                            <thead>
                                <tr>
                                    <th>Источник</th>
                                    <th>Тикер</th>
                                    <th>Цена</th>
                                    <th>Объем</th>
                                    <th>Timestamp</th>
                                    <th>Received</th>
                                </tr>
                            </thead>
                            <tbody id="ticks-body"></tbody>
                        </table>
                        <div class="empty" id="ticks-empty">Пока нет записанных тиков.</div>
                    </article>
                </section>
            </main>

            <script>
                const statsContainer = document.getElementById("stats");
                const simulatorsContainer = document.getElementById("simulators");
                const ticksBody = document.getElementById("ticks-body");
                const ticksEmpty = document.getElementById("ticks-empty");
                const lastUpdate = document.getElementById("last-update");
                const sourcesCount = document.getElementById("sources-count");
                const writeBadge = document.getElementById("write-badge");
                const actionStatus = document.getElementById("action-status");

                const statTitles = [
                    ["receivedTicks", "Получено"],
                    ["processedTicks", "Обработано"],
                    ["writtenTicks", "Записано"],
                    ["duplicateTicks", "Дубликаты"],
                    ["droppedTicks", "Сброшено"],
                    ["pendingTicks", "В очереди"]
                ];

                let simulatorMap = new Map();

                function formatNumber(value) {
                    return new Intl.NumberFormat("ru-RU").format(value ?? 0);
                }

                function formatDate(value) {
                    if (!value) {
                        return "-";
                    }

                    return new Date(value).toLocaleString("ru-RU");
                }

                function renderStats(status) {
                    statsContainer.innerHTML = "";

                    for (const [key, title] of statTitles) {
                        const card = document.createElement("article");
                        card.className = "card";
                        card.innerHTML = `
                            <div class="stat-title">${title}</div>
                            <div class="stat-value">${formatNumber(status[key])}</div>
                        `;
                        statsContainer.appendChild(card);
                    }

                    writeBadge.textContent = `Записано: ${formatNumber(status.writtenTicks)}`;
                    writeBadge.className = status.droppedTicks > 0 ? "badge badge-danger" : "badge";
                }

                function renderSimulators(simulators) {
                    simulatorMap = new Map(simulators.map(x => [x.source, x]));
                    sourcesCount.textContent = `Источники: ${simulators.length}`;
                    simulatorsContainer.innerHTML = "";

                    for (const simulator of simulators) {
                        const state = simulator.state ?? {
                            duplicatesEnabled: false,
                            duplicateChance: 0,
                            silenceEnabled: false,
                            silenceDurationMs: null,
                            silenceAfterMessages: null,
                            outageEnabled: false,
                            outageDurationMs: null,
                            outageAfterMessages: null,
                            cancelOutageRequested: false,
                            disconnectMode: "None",
                            disconnectAfterMessages: null
                        };

                        const availabilityBadge = simulator.isUnavailable
                            ? `<span class="badge badge-danger">WS недоступен</span>`
                            : simulator.isAvailable
                                ? `<span class="badge">Доступен</span>`
                                : `<span class="badge badge-danger">Недоступен</span>`;

                        const errorBlock = simulator.error
                            ? `<div class="error">${simulator.error}</div>`
                            : "";

                        const card = document.createElement("article");
                        card.className = "card simulator-card";
                        card.innerHTML = `
                            <div class="simulator-top">
                                <div>
                                    <h2 class="simulator-name">${simulator.source}</h2>
                                    <div class="simulator-kind">Формат: ${simulator.kind}</div>
                                    <div class="simulator-url">${simulator.url}</div>
                                </div>
                                ${availabilityBadge}
                            </div>

                            <div class="simulator-metrics">
                                <div class="metric-box">
                                    <div class="metric-label">Активные подключения</div>
                                    <div class="metric-value">${formatNumber(simulator.activeConnections)}</div>
                                </div>
                                <div class="metric-box">
                                    <div class="metric-label">Сгенерировано</div>
                                    <div class="metric-value">${formatNumber(simulator.totalGeneratedTicks)}</div>
                                </div>
                                <div class="metric-box">
                                    <div class="metric-label">Отправлено</div>
                                    <div class="metric-value">${formatNumber(simulator.totalSentMessages)}</div>
                                </div>
                                <div class="metric-box">
                                    <div class="metric-label">Дубликатов отправлено</div>
                                    <div class="metric-value">${formatNumber(simulator.totalDuplicateMessages)}</div>
                                </div>
                            </div>

                            <div class="simulator-state">
                                <div class="simulator-state-line">Последний тикер: ${simulator.lastSymbol ?? "-"}</div>
                                <div class="simulator-state-line">Последняя отправка: ${formatDate(simulator.lastSentAt)}</div>
                                <div class="simulator-state-line">Дубликаты: ${state.duplicatesEnabled ? `включены (${state.duplicateChance})` : "выключены"}</div>
                                <div class="simulator-state-line">Тишина: ${state.silenceEnabled ? `включена (${state.silenceDurationMs} ms)` : "выключена"}</div>
                                <div class="simulator-state-line">Недоступность: ${simulator.isUnavailable ? `до ${formatDate(simulator.unavailableUntil)}` : "нет"}</div>
                                <div class="simulator-state-line">Разрыв: ${state.disconnectMode}${state.disconnectAfterMessages ? ` (${state.disconnectAfterMessages})` : ""}</div>
                            </div>

                            <div class="actions">
                                <button class="btn btn-primary" onclick="applyPreset('${simulator.source}', 'duplicatesOn')">Дубликаты 30%</button>
                                <button class="btn btn-soft" onclick="applyPreset('${simulator.source}', 'duplicatesOff')">Убрать дубликаты</button>
                                <button class="btn btn-danger" onclick="applyPreset('${simulator.source}', 'disconnectNext')">Разрыв next</button>
                                <button class="btn btn-danger" onclick="applyPreset('${simulator.source}', 'disconnectAfterFive')">Разрыв через 5</button>
                                <button class="btn btn-danger" onclick="applyPreset('${simulator.source}', 'outageMinute')">Умереть на 1 минуту</button>
                                <button class="btn btn-soft" onclick="applyPreset('${simulator.source}', 'silenceNext')">Тишина 5 сек</button>
                                <button class="btn btn-soft" onclick="applyPreset('${simulator.source}', 'reset')">Сброс</button>
                            </div>
                            <div class="hint">Для теста дубликатов сначала включи дубликаты на одном из симуляторов и подожди 2-3 батча записи.</div>
                            ${errorBlock}
                        `;

                        simulatorsContainer.appendChild(card);
                    }
                }

                function renderTicks(ticks) {
                    ticksBody.innerHTML = "";
                    ticksEmpty.style.display = ticks.length === 0 ? "block" : "none";

                    for (const tick of ticks) {
                        const row = document.createElement("tr");
                        row.innerHTML = `
                            <td>${tick.source}</td>
                            <td>${tick.symbol}</td>
                            <td>${tick.price}</td>
                            <td>${tick.volume}</td>
                            <td>${formatDate(tick.timestamp)}</td>
                            <td>${formatDate(tick.receivedAt)}</td>
                        `;
                        ticksBody.appendChild(row);
                    }
                }

                async function refresh() {
                    const response = await fetch("/api/dashboard", { cache: "no-store" });
                    const data = await response.json();

                    renderStats(data.status);
                    renderSimulators(data.simulators);
                    renderTicks(data.recentTicks);
                    lastUpdate.textContent = `Обновление: ${new Date().toLocaleTimeString("ru-RU")}`;
                }

                function createResetState() {
                    return {
                        duplicatesEnabled: false,
                        duplicateChance: 0,
                        silenceEnabled: false,
                        silenceDurationMs: null,
                        silenceAfterMessages: null,
                        outageEnabled: false,
                        outageDurationMs: null,
                        outageAfterMessages: null,
                        cancelOutageRequested: true,
                        disconnectMode: "None",
                        disconnectAfterMessages: null
                    };
                }

                function applyPresetState(current, preset) {
                    const next = structuredClone(current ?? createResetState());

                    switch (preset) {
                        case "duplicatesOn":
                            next.duplicatesEnabled = true;
                            next.duplicateChance = 0.3;
                            return next;
                        case "duplicatesOff":
                            next.duplicatesEnabled = false;
                            next.duplicateChance = 0;
                            return next;
                        case "disconnectNext":
                            next.disconnectMode = "AfterNextMessage";
                            next.disconnectAfterMessages = null;
                            return next;
                        case "disconnectAfterFive":
                            next.disconnectMode = "AfterMessageCount";
                            next.disconnectAfterMessages = 5;
                            return next;
                        case "outageMinute":
                            next.outageEnabled = true;
                            next.outageDurationMs = 60000;
                            next.outageAfterMessages = null;
                            return next;
                        case "silenceNext":
                            next.silenceEnabled = true;
                            next.silenceDurationMs = 5000;
                            next.silenceAfterMessages = 1;
                            return next;
                        case "reset":
                            return createResetState();
                        default:
                            return next;
                    }
                }

                async function applyPreset(source, preset) {
                    const simulator = simulatorMap.get(source);
                    if (!simulator) {
                        return;
                    }

                    const state = applyPresetState(simulator.state, preset);

                    const response = await fetch(`/api/simulators/${source}/state`, {
                        method: "POST",
                        headers: {
                            "Content-Type": "application/json"
                        },
                        body: JSON.stringify(state)
                    });

                    if (!response.ok) {
                        const text = await response.text();
                        alert(`Не удалось обновить состояние симулятора ${source}: ${text}`);
                        return;
                    }

                    actionStatus.textContent = `Сценарий ${preset} применен к ${source}.`;
                    await refresh();
                }

                async function applyPresetToAll(preset) {
                    const simulators = Array.from(simulatorMap.values());
                    if (simulators.length === 0) {
                        return;
                    }

                    actionStatus.textContent = "Применяю сценарий ко всем симуляторам...";

                    for (const simulator of simulators) {
                        const state = applyPresetState(simulator.state, preset);
                        const response = await fetch(`/api/simulators/${simulator.source}/state`, {
                            method: "POST",
                            headers: {
                                "Content-Type": "application/json"
                            },
                            body: JSON.stringify(state)
                        });

                        if (!response.ok) {
                            const text = await response.text();
                            actionStatus.textContent = `Ошибка при обновлении ${simulator.source}: ${text}`;
                            return;
                        }
                    }

                    actionStatus.textContent = `Сценарий ${preset} применен ко всем симуляторам.`;
                    await refresh();
                }

                refresh();
                setInterval(refresh, 1000);
            </script>
        </body>
        </html>
        """;
}
