using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LJokerL.Core.Models;

namespace LJokerL.Core.Services;

public static class BenchmarkReportGenerator
{
    public static (string HtmlPath, string MdPath) GenerateReport(BenchmarkRunResult before, BenchmarkRunResult after, string outputDirectory)
    {
        Directory.CreateDirectory(outputDirectory);

        var oldCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
        var oldUiCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;

        try
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;

            var comparison = BenchmarkComparer.Compare(before, after);
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            
            var htmlPath = Path.Combine(outputDirectory, $"BenchmarkReport_{timestamp}.html");
            var mdPath = Path.Combine(outputDirectory, $"BenchmarkReport_{timestamp}.md");

            var htmlContent = GenerateHtmlReport(before, after, comparison);
            var mdContent = GenerateMarkdownReport(before, after, comparison);

            File.WriteAllText(htmlPath, htmlContent, Encoding.UTF8);
            File.WriteAllText(mdPath, mdContent, Encoding.UTF8);

            return (htmlPath, mdPath);
        }
        finally
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = oldUiCulture;
        }
    }

    private static string GenerateHtmlReport(BenchmarkRunResult before, BenchmarkRunResult after, BenchmarkComparisonResult comparison)
    {
        var cpuName = after.Cpu?.CPUName ?? before.Cpu?.CPUName ?? "Unknown Processor";
        var gpuName = after.System?.ComputerModel ?? before.System?.ComputerModel ?? "Dedicated GPU";
        var ramTotal = after.Memory != null ? $"{after.Memory.TotalRAM} GB" : (before.Memory != null ? $"{before.Memory.TotalRAM} GB" : "Unknown RAM");
        var osVersion = after.System?.OSName ?? before.System?.OSName ?? "Windows 11";
        var biosVersion = after.System?.BIOSVersion ?? before.System?.BIOSVersion ?? "N/A";

        var overallBeforeStr = comparison.OverallBefore?.ToString("F1") ?? "N/A";
        var overallAfterStr = comparison.OverallAfter?.ToString("F1") ?? "N/A";
        var overallDelta = comparison.OverallDelta ?? 0.0;
        var overallPct = comparison.OverallPercentChange ?? 0.0;
        var overallDeltaClass = overallDelta >= 0 ? "positive" : "negative";
        var overallDeltaSign = overallDelta >= 0 ? "+" : "";

        // Jitter / Latency metrics (Lower is better)
        var cpuJitterBeforeStr = comparison.CpuJitterBefore?.ToString("F3") ?? "N/A";
        var cpuJitterAfterStr = comparison.CpuJitterAfter?.ToString("F3") ?? "N/A";
        var cpuJitterDelta = comparison.CpuJitterDelta ?? 0.0;
        var cpuJitterPct = comparison.CpuJitterPercentChange ?? 0.0;
        var cpuJitterClass = cpuJitterDelta <= 0 ? "positive" : "negative"; // Negative delta is positive improvement
        var cpuJitterSign = cpuJitterDelta >= 0 ? "+" : "";

        var memLatBeforeStr = comparison.MemLatencyBefore?.ToString("F2") ?? "N/A";
        var memLatAfterStr = comparison.MemLatencyAfter?.ToString("F2") ?? "N/A";
        var memLatDelta = comparison.MemLatencyDelta ?? 0.0;
        var memLatPct = comparison.MemLatencyPercentChange ?? 0.0;
        var memLatClass = memLatDelta <= 0 ? "positive" : "negative";
        var memLatSign = memLatDelta >= 0 ? "+" : "";

        var netJitterBeforeStr = comparison.NetJitterBefore?.ToString("F2") ?? "N/A";
        var netJitterAfterStr = comparison.NetJitterAfter?.ToString("F2") ?? "N/A";
        var netJitterDelta = comparison.NetJitterDelta ?? 0.0;
        var netJitterPct = comparison.NetJitterPercentChange ?? 0.0;
        var netJitterClass = netJitterDelta <= 0 ? "positive" : "negative";
        var netJitterSign = netJitterDelta >= 0 ? "+" : "";

        var sb = new StringBuilder();
        sb.Append($@"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Gaming Optimization Performance Report</title>
    <link href=""https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&family=Outfit:wght@400;600;700;800&display=swap"" rel=""stylesheet"">
    <style>
        :root {{
            --bg-color: #0b0d19;
            --card-bg: rgba(20, 24, 46, 0.6);
            --border-color: rgba(255, 255, 255, 0.06);
            --text-primary: #f3f4f6;
            --text-secondary: #9ca3af;
            --primary: #00f2fe;
            --secondary: #4facfe;
            --accent-green: #10b981;
            --accent-red: #ef4444;
            --font-family: 'Inter', sans-serif;
            --heading-font: 'Outfit', sans-serif;
        }}

        * {{
            box-sizing: border-box;
            margin: 0;
            padding: 0;
        }}

        body {{
            background-color: var(--bg-color);
            color: var(--text-primary);
            font-family: var(--font-family);
            line-height: 1.6;
            padding: 2.5rem 1.5rem;
            background-image: radial-gradient(circle at 10% 20%, rgba(0, 242, 254, 0.05) 0%, transparent 40%),
                              radial-gradient(circle at 90% 80%, rgba(79, 172, 254, 0.05) 0%, transparent 40%);
            background-attachment: fixed;
        }}

        .container {{
            max-width: 1200px;
            margin: 0 auto;
        }}

        header {{
            text-align: center;
            margin-bottom: 3.5rem;
        }}

        .badge-pro {{
            display: inline-block;
            background: linear-gradient(135deg, rgba(0, 242, 254, 0.15), rgba(79, 172, 254, 0.15));
            border: 1px solid rgba(0, 242, 254, 0.3);
            color: var(--primary);
            font-size: 0.8rem;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 0.15rem;
            padding: 0.4rem 1rem;
            border-radius: 50px;
            margin-bottom: 1rem;
            font-family: var(--heading-font);
        }}

        h1 {{
            font-family: var(--heading-font);
            font-size: 2.8rem;
            font-weight: 800;
            background: linear-gradient(135deg, #00f2fe 0%, #4facfe 100%);
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
            margin-bottom: 0.5rem;
            letter-spacing: -0.05rem;
        }}

        .subtitle {{
            color: var(--text-secondary);
            font-size: 1.1rem;
            font-weight: 400;
        }}

        /* System Details Card */
        .system-details-card {{
            background: var(--card-bg);
            border: 1px solid var(--border-color);
            border-radius: 16px;
            padding: 1.5rem 2rem;
            margin-bottom: 2.5rem;
            backdrop-filter: blur(12px);
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 1.5rem;
        }}

        .system-prop {{
            display: flex;
            flex-direction: column;
        }}

        .system-prop .label {{
            color: var(--text-secondary);
            font-size: 0.8rem;
            text-transform: uppercase;
            letter-spacing: 0.05rem;
            margin-bottom: 0.3rem;
            font-weight: 500;
        }}

        .system-prop .value {{
            color: var(--text-primary);
            font-weight: 600;
            font-size: 0.95rem;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }}

        /* Core Score Overview */
        .dashboard-grid {{
            display: grid;
            grid-template-columns: 2fr 3fr;
            gap: 2rem;
            margin-bottom: 2.5rem;
        }}

        @media (max-width: 900px) {{
            .dashboard-grid {{
                grid-template-columns: 1fr;
            }}
        }}

        .panel {{
            background: var(--card-bg);
            border: 1px solid var(--border-color);
            border-radius: 20px;
            padding: 2rem;
            backdrop-filter: blur(12px);
            position: relative;
            overflow: hidden;
        }}

        .panel::before {{
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 4px;
            background: linear-gradient(90deg, var(--primary), var(--secondary));
        }}

        .panel-title {{
            font-family: var(--heading-font);
            font-size: 1.4rem;
            font-weight: 700;
            margin-bottom: 1.5rem;
            display: flex;
            align-items: center;
            gap: 0.5rem;
        }}

        /* Overall Score Circular Design */
        .score-comparison-wrapper {{
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            height: 100%;
            padding-top: 1rem;
        }}

        .score-ring-container {{
            position: relative;
            width: 180px;
            height: 180px;
            display: flex;
            align-items: center;
            justify-content: center;
            margin-bottom: 1.5rem;
        }}

        .score-ring-svg {{
            width: 100%;
            height: 100%;
            transform: rotate(-90deg);
        }}

        .score-ring-bg {{
            fill: none;
            stroke: rgba(255, 255, 255, 0.03);
            stroke-width: 12;
        }}

        .score-ring-fill {{
            fill: none;
            stroke: url(#score-grad);
            stroke-width: 12;
            stroke-dasharray: 502;
            stroke-dashoffset: {502 - (502 * (comparison.OverallAfter ?? 0) / 100)};
            stroke-linecap: round;
            transition: stroke-dashoffset 1.5s ease-out;
        }}

        .score-value {{
            position: absolute;
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
        }}

        .score-number {{
            font-family: var(--heading-font);
            font-size: 3rem;
            font-weight: 800;
            color: var(--text-primary);
            line-height: 1;
        }}

        .score-label {{
            color: var(--text-secondary);
            font-size: 0.75rem;
            text-transform: uppercase;
            letter-spacing: 0.1rem;
            margin-top: 0.2rem;
        }}

        .overall-delta-badge {{
            display: inline-flex;
            align-items: center;
            gap: 0.3rem;
            padding: 0.4rem 1rem;
            border-radius: 50px;
            font-weight: 700;
            font-size: 0.95rem;
        }}

        .overall-delta-badge.positive {{
            background: rgba(16, 185, 129, 0.12);
            color: var(--accent-green);
            border: 1px solid rgba(16, 185, 129, 0.25);
        }}

        .overall-delta-badge.negative {{
            background: rgba(239, 68, 68, 0.12);
            color: var(--accent-red);
            border: 1px solid rgba(239, 68, 68, 0.25);
        }}

        /* Gaming Latency Cards */
        .latency-metrics-grid {{
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 1rem;
        }}

        @media (max-width: 600px) {{
            .latency-metrics-grid {{
                grid-template-columns: 1fr;
            }}
        }}

        .latency-card {{
            background: rgba(255, 255, 255, 0.02);
            border: 1px solid rgba(255, 255, 255, 0.04);
            border-radius: 12px;
            padding: 1.25rem;
            text-align: center;
            transition: all 0.3s ease;
        }}

        .latency-card:hover {{
            background: rgba(255, 255, 255, 0.03);
            border-color: rgba(0, 242, 254, 0.2);
            transform: translateY(-2px);
        }}

        .latency-name {{
            font-size: 0.8rem;
            color: var(--text-secondary);
            text-transform: uppercase;
            letter-spacing: 0.04rem;
            margin-bottom: 0.75rem;
            font-weight: 500;
        }}

        .latency-values {{
            display: flex;
            align-items: baseline;
            justify-content: center;
            gap: 0.5rem;
            margin-bottom: 0.5rem;
        }}

        .latency-before {{
            font-size: 0.95rem;
            color: var(--text-secondary);
            text-decoration: line-through;
        }}

        .latency-after {{
            font-family: var(--heading-font);
            font-size: 1.6rem;
            font-weight: 700;
            color: var(--text-primary);
        }}

        .latency-delta {{
            font-size: 0.8rem;
            font-weight: 700;
            display: inline-flex;
            align-items: center;
            padding: 0.15rem 0.5rem;
            border-radius: 4px;
        }}

        .latency-delta.positive {{
            color: var(--accent-green);
            background: rgba(16, 185, 129, 0.08);
        }}

        .latency-delta.negative {{
            color: var(--accent-red);
            background: rgba(239, 68, 68, 0.08);
        }}

        /* Detailed Categories Table */
        .categories-panel {{
            margin-bottom: 2.5rem;
        }}

        table {{
            width: 100%;
            border-collapse: collapse;
            margin-top: 1rem;
        }}

        th {{
            text-align: left;
            padding: 1rem;
            color: var(--text-secondary);
            font-size: 0.85rem;
            text-transform: uppercase;
            letter-spacing: 0.05rem;
            border-bottom: 1px solid var(--border-color);
            font-weight: 600;
        }}

        td {{
            padding: 1.25rem 1rem;
            border-bottom: 1px solid rgba(255, 255, 255, 0.03);
            font-size: 0.95rem;
        }}

        tr:hover td {{
            background: rgba(255, 255, 255, 0.01);
        }}

        .category-name {{
            font-weight: 600;
            color: var(--text-primary);
            font-family: var(--heading-font);
        }}

        .score-val-table {{
            font-weight: 500;
        }}

        .table-delta {{
            font-weight: 700;
        }}

        .table-delta.positive {{
            color: var(--accent-green);
        }}

        .table-delta.negative {{
            color: var(--accent-red);
        }}

        /* Gaming Impact Analysis */
        .impact-grid {{
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
            gap: 1.5rem;
        }}

        .impact-card {{
            background: rgba(255, 255, 255, 0.02);
            border: 1px solid rgba(255, 255, 255, 0.04);
            border-radius: 16px;
            padding: 1.5rem;
            transition: all 0.3s ease;
        }}

        .impact-card:hover {{
            border-color: rgba(79, 172, 254, 0.2);
            background: rgba(255, 255, 255, 0.03);
        }}

        .impact-header {{
            display: flex;
            align-items: center;
            gap: 0.75rem;
            margin-bottom: 1rem;
        }}

        .impact-icon {{
            width: 32px;
            height: 32px;
            border-radius: 8px;
            background: linear-gradient(135deg, var(--primary), var(--secondary));
            display: flex;
            align-items: center;
            justify-content: center;
            color: #0b0d19;
            font-weight: 800;
            font-family: var(--heading-font);
            font-size: 1.1rem;
        }}

        .impact-title {{
            font-family: var(--heading-font);
            font-size: 1.1rem;
            font-weight: 600;
            color: var(--text-primary);
        }}

        .impact-desc {{
            color: var(--text-secondary);
            font-size: 0.88rem;
            line-height: 1.5;
        }}

        /* Footer */
        footer {{
            text-align: center;
            margin-top: 4rem;
            color: var(--text-secondary);
            font-size: 0.85rem;
            border-top: 1px solid var(--border-color);
            padding-top: 2rem;
        }}

        footer a {{
            color: var(--primary);
            text-decoration: none;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <header>
            <div class=""badge-pro"">System Benchmark Report</div>
            <h1>Optimization Performance Report</h1>
            <div class=""subtitle"">Gaming and latency benchmark comparison</div>
        </header>

        <!-- System Information -->
        <div class=""system-details-card"">
            <div class=""system-prop"">
                <span class=""label"">Processor</span>
                <span class=""value"" title=""{cpuName}"">{cpuName}</span>
            </div>
            <div class=""system-prop"">
                <span class=""label"">System Model</span>
                <span class=""value"" title=""{gpuName}"">{gpuName}</span>
            </div>
            <div class=""system-prop"">
                <span class=""label"">Total Memory</span>
                <span class=""value"">{ramTotal}</span>
            </div>
            <div class=""system-prop"">
                <span class=""label"">OS Version</span>
                <span class=""value"">{osVersion}</span>
            </div>
            <div class=""system-prop"">
                <span class=""label"">BIOS Version</span>
                <span class=""value"" title=""{biosVersion}"">{biosVersion}</span>
            </div>
        </div>

        <div class=""dashboard-grid"">
            <!-- Overall Score Ring Panel -->
            <div class=""panel"">
                <div class=""panel-title"">
                    <span>Overall Score Comparison</span>
                </div>
                <div class=""score-comparison-wrapper"">
                    <div class=""score-ring-container"">
                        <svg class=""score-ring-svg"">
                            <defs>
                                <linearGradient id=""score-grad"" x1=""0%"" y1=""0%"" x2=""100%"" y2=""100%"">
                                    <stop offset=""0%"" stop-color=""#00f2fe"" />
                                    <stop offset=""100%"" stop-color=""#4facfe"" />
                                </linearGradient>
                            </defs>
                            <circle class=""score-ring-bg"" cx=""90"" cy=""90"" r=""80"" />
                            <circle class=""score-ring-fill"" cx=""90"" cy=""90"" r=""80"" />
                        </svg>
                        <div class=""score-value"">
                            <span class=""score-number"">{overallAfterStr}</span>
                            <span class=""score-label"">After Score</span>
                        </div>
                    </div>
                    <div>
                        <span class=""overall-delta-badge {overallDeltaClass}"">
                            Before: {overallBeforeStr} &rarr; After: {overallAfterStr} ({overallDeltaSign}{overallPct}%)
                        </span>
                    </div>
                </div>
            </div>

            <!-- Gaming Latency Metrics Panel -->
            <div class=""panel"">
                <div class=""panel-title"">
                    <span>Gaming Latency & Jitter Profiles</span>
                </div>
                <div class=""latency-metrics-grid"">
                    <!-- CPU Scheduling Jitter -->
                    <div class=""latency-card"">
                        <div class=""latency-name"">CPU Thread Jitter</div>
                        <div class=""latency-values"">
                            <span class=""latency-before"">{cpuJitterBeforeStr}ms</span>
                            <span class=""latency-after"">{cpuJitterAfterStr}ms</span>
                        </div>
                        <span class=""latency-delta {cpuJitterClass}"">
                            {cpuJitterDelta:F3}ms ({cpuJitterPct:F1}%)
                        </span>
                    </div>

                    <!-- Memory Latency -->
                    <div class=""latency-card"">
                        <div class=""latency-name"">DRAM Latency</div>
                        <div class=""latency-values"">
                            <span class=""latency-before"">{memLatBeforeStr}ns</span>
                            <span class=""latency-after"">{memLatAfterStr}ns</span>
                        </div>
                        <span class=""latency-delta {memLatClass}"">
                            {memLatDelta:F2}ns ({memLatPct:F1}%)
                        </span>
                    </div>

                    <!-- Network Ping Jitter -->
                    <div class=""latency-card"">
                        <div class=""latency-name"">Network Jitter</div>
                        <div class=""latency-values"">
                            <span class=""latency-before"">{netJitterBeforeStr}ms</span>
                            <span class=""latency-after"">{netJitterAfterStr}ms</span>
                        </div>
                        <span class=""latency-delta {netJitterClass}"">
                            {netJitterDelta:F2}ms ({netJitterPct:F1}%)
                        </span>
                    </div>
                </div>
            </div>
        </div>

        <!-- Detailed Comparison Table -->
        <div class=""panel categories-panel"">
            <div class=""panel-title"">Detailed Category Scores</div>
            <table>
                <thead>
                    <tr>
                        <th>Category</th>
                        <th>Before Score</th>
                        <th>After Score</th>
                        <th>Delta Improvement</th>
                        <th>Percentage</th>
                    </tr>
                </thead>
                <tbody>");

        foreach (var delta in comparison.Deltas)
        {
            var deltaVal = delta.Delta ?? 0.0;
            var pctVal = delta.PercentChange ?? 0.0;
            var classStr = deltaVal >= 0 ? "positive" : "negative";
            var signStr = deltaVal >= 0 ? "+" : "";

            sb.Append($@"
                    <tr>
                        <td class=""category-name"">{delta.Name}</td>
                        <td class=""score-val-table"">{delta.BeforeScore?.ToString("F0") ?? "N/A"}</td>
                        <td class=""score-val-table"">{delta.AfterScore?.ToString("F0") ?? "N/A"}</td>
                        <td class=""table-delta {classStr}"">{signStr}{deltaVal:F0}</td>
                        <td class=""table-delta {classStr}"">{signStr}{pctVal:F1}%</td>
                    </tr>");
        }

        sb.Append($@"
                </tbody>
            </table>
        </div>

        <!-- Gaming Impact Analysis Section -->
        <div class=""panel"">
            <div class=""panel-title"">Gaming & System Real-world Impact</div>
            <div class=""impact-grid"">
                <div class=""impact-card"">
                    <div class=""impact-header"">
                        <div class=""impact-icon"">T</div>
                        <h3 class=""impact-title"">Frametime & 1% Low FPS</h3>
                    </div>
                    <p class=""impact-desc"">
                        Reducing CPU Thread Jitter ensures the OS scheduler dispatches gaming threads instantly. Lower thread scheduling jitter directly translates to massive reductions in micro-stuttering and stabilizes your 1% and 0.1% low framerates.
                    </p>
                </div>
                <div class=""impact-card"">
                    <div class=""impact-header"">
                        <div class=""impact-icon"">M</div>
                        <h3 class=""impact-title"">Input Lag & Memory Response</h3>
                    </div>
                    <p class=""impact-desc"">
                        True random pointer-chasing latency measurements reflect memory access response times. Optimizing memory sub-timings, disabling CPU throttling state delays, and cleaning up cache pollution keeps memory access latencies incredibly low.
                    </p>
                </div>
                <div class=""impact-card"">
                    <div class=""impact-header"">
                        <div class=""impact-icon"">N</div>
                        <h3 class=""impact-title"">Netcode Registry Optimization</h3>
                    </div>
                    <p class=""impact-desc"">
                        Disabling network throttling and optimizing Nagle's algorithm yields much lower standard deviation in ping times. A jitter-free network connection guarantees hit registration consistency in fast-paced shooter environments.
                    </p>
                </div>
            </div>
        </div>

        <footer>
            <p>Generated by <strong>LJokerL_TTV Custom Config PRO</strong> &bull; Latency & Gaming Systems Optimization Engine</p>
        </footer>
    </div>
</body>
</html>");

        return sb.ToString();
    }

    private static string GenerateMarkdownReport(BenchmarkRunResult before, BenchmarkRunResult after, BenchmarkComparisonResult comparison)
    {
        var cpuName = after.Cpu?.CPUName ?? before.Cpu?.CPUName ?? "Unknown Processor";
        var gpuName = after.System?.ComputerModel ?? before.System?.ComputerModel ?? "Dedicated GPU";
        var ramTotal = after.Memory != null ? $"{after.Memory.TotalRAM} GB" : (before.Memory != null ? $"{before.Memory.TotalRAM} GB" : "Unknown RAM");
        var osVersion = after.System?.OSName ?? before.System?.OSName ?? "Windows 11";
        
        var overallBefore = comparison.OverallBefore?.ToString("F1") ?? "N/A";
        var overallAfter = comparison.OverallAfter?.ToString("F1") ?? "N/A";
        var overallPct = comparison.OverallPercentChange.HasValue ? $"{(comparison.OverallPercentChange >= 0 ? "+" : "")}{comparison.OverallPercentChange:F1}%" : "0.0%";

        var sb = new StringBuilder();
        sb.AppendLine("# Gaming Optimization & System Performance Report");
        sb.AppendLine();
        sb.AppendLine($"> [!NOTE]");
        sb.AppendLine($"> **System Specifications:**");
        sb.AppendLine($"> - **CPU:** {cpuName}");
        sb.AppendLine($"> - **GPU/Model:** {gpuName}");
        sb.AppendLine($"> - **Memory:** {ramTotal}");
        sb.AppendLine($"> - **Operating System:** {osVersion}");
        sb.AppendLine();
        sb.AppendLine("## Executive Performance Summary");
        sb.AppendLine();
        sb.AppendLine($"**Overall Score Progression: {overallBefore} &rarr; {overallAfter} ({overallPct})**");
        sb.AppendLine();
        sb.AppendLine("| Metric Category | Before Run | After Run | Improvement Delta | Percentage |");
        sb.AppendLine("| :--- | :---: | :---: | :---: | :---: |");

        foreach (var delta in comparison.Deltas)
        {
            var deltaVal = delta.Delta ?? 0.0;
            var pctVal = delta.PercentChange ?? 0.0;
            var signStr = deltaVal >= 0 ? "+" : "";
            sb.AppendLine($"| **{delta.Name}** | {delta.BeforeScore?.ToString("F0") ?? "N/A"} | {delta.AfterScore?.ToString("F0") ?? "N/A"} | {signStr}{deltaVal:F0} | {signStr}{pctVal:F1}% |");
        }

        sb.AppendLine();
        sb.AppendLine("## Gaming Latency & Jitter Profiles");
        sb.AppendLine();
        sb.AppendLine("| Low-level Timing Metric | Before Run | After Run | Net Improvement | Percentage Change |");
        sb.AppendLine("| :--- | :---: | :---: | :---: | :---: |");

        // CPU Jitter
        if (comparison.CpuJitterBefore.HasValue && comparison.CpuJitterAfter.HasValue)
        {
            var diff = comparison.CpuJitterAfter.Value - comparison.CpuJitterBefore.Value;
            var pct = comparison.CpuJitterPercentChange ?? 0.0;
            sb.AppendLine($"| **CPU Thread Scheduling Jitter** | {comparison.CpuJitterBefore:F3} ms | {comparison.CpuJitterAfter:F3} ms | {diff:F3} ms | {(pct >= 0 ? "+" : "")}{pct:F1}% |");
        }

        // Memory Pointer Chasing
        if (comparison.MemLatencyBefore.HasValue && comparison.MemLatencyAfter.HasValue)
        {
            var diff = comparison.MemLatencyAfter.Value - comparison.MemLatencyBefore.Value;
            var pct = comparison.MemLatencyPercentChange ?? 0.0;
            sb.AppendLine($"| **Memory Pointer-Chasing Latency** | {comparison.MemLatencyBefore:F2} ns | {comparison.MemLatencyAfter:F2} ns | {diff:F2} ns | {(pct >= 0 ? "+" : "")}{pct:F1}% |");
        }

        // Network Jitter
        if (comparison.NetJitterBefore.HasValue && comparison.NetJitterAfter.HasValue)
        {
            var diff = comparison.NetJitterAfter.Value - comparison.NetJitterBefore.Value;
            var pct = comparison.NetJitterPercentChange ?? 0.0;
            sb.AppendLine($"| **Network Ping Jitter** | {comparison.NetJitterBefore:F2} ms | {comparison.NetJitterAfter:F2} ms | {diff:F2} ms | {(pct >= 0 ? "+" : "")}{pct:F1}% |");
        }

        sb.AppendLine();
        sb.AppendLine("## Real-world Gaming Impact");
        sb.AppendLine();
        sb.AppendLine("### 1. Frametime Stability & 1% Lows");
        sb.AppendLine("Reducing CPU scheduling/thread jitter ensures the game engine is never waiting on deferred thread execution. This prevents random frametime spikes and input-lag stutters.");
        sb.AppendLine();
        sb.AppendLine("### 2. Physics & Input Response");
        sb.AppendLine("Memory pointer-chasing tests measure the pure random latency of the memory subsystem. Minimizing this metric improves reaction time in high-APM situations.");
        sb.AppendLine();
        sb.AppendLine("### 3. Hit Registration (Netcode)");
        sb.AppendLine("A lower ping standard deviation (network jitter) provides a smoother path for server updates, reducing hit registration delay and netcode discrepancies.");
        sb.AppendLine();
        sb.AppendLine("---");
        sb.AppendLine("Report generated by **LJokerL_TTV Custom Config PRO** Gaming Optimizer.");

        return sb.ToString();
    }
}
