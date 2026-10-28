using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using LJokerL.Core.Models;
using LJokerL.Core.Services;

namespace LJokerL.App.ViewModels;

public sealed class MainViewModel : ObservableObject
{
    public string Title { get; } = "LJokerL Control Center";
    public string Subtitle { get; } = "Unified gaming optimization and profiles";

    public ObservableCollection<ProfileItem> Profiles { get; } = new();
    public ObservableCollection<TweakItem> AdvancedTweaks { get; } = new();
    public ObservableCollection<RecommendationItem> Recommendations { get; } = new();
    public ObservableCollection<HardwareFlagItem> HardwareFlags { get; } = new();
    public ObservableCollection<LogEntryItem> ExecutionLogs { get; } = new();
    public ObservableCollection<ProcessUsageItem> MonitoringProcesses { get; } = new();
    public ObservableCollection<BenchmarkRunItem> BenchmarkRuns { get; } = new();

    public ObservableCollection<KeyValuePair<string, string>> RiskFilters { get; } = new();
    public ObservableCollection<KeyValuePair<string, string>> CategoryFilters { get; } = new();
    public ObservableCollection<KeyValuePair<string, string>> VrrOverrideOptions { get; } = new();
    public ObservableCollection<KeyValuePair<string, string>> LogStatusFilters { get; } = new();
    public ObservableCollection<KeyValuePair<string, string>> LogTypeFilters { get; } = new();
    public ObservableCollection<int> LogPageSizeOptions { get; } = new();

    private readonly List<TweakItem> _allTweaks = new();
    private readonly List<LogEntryItem> _allLogItems = new();

    private ProfileItem? _selectedProfile;
    public ProfileItem? SelectedProfile
    {
        get => _selectedProfile;
        set
        {
            _selectedProfile = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsProfileSelected));
            _applySelectedProfileCommand?.RaiseCanExecuteChanged();
        }
    }

    public bool IsProfileSelected => SelectedProfile != null;

    private TweakItem? _selectedTweak;
    public TweakItem? SelectedTweak
    {
        get => _selectedTweak;
        set
        {
            _selectedTweak = value;
            OnPropertyChanged();
            _applySelectedTweakCommand?.RaiseCanExecuteChanged();
            _revertSelectedTweakCommand?.RaiseCanExecuteChanged();
        }
    }

    private string _tweakSearchText = string.Empty;
    public string TweakSearchText
    {
        get => _tweakSearchText;
        set
        {
            _tweakSearchText = value;
            OnPropertyChanged();
            RefreshTweakList();
        }
    }

    private string _selectedRiskFilter = string.Empty;
    public string SelectedRiskFilter
    {
        get => _selectedRiskFilter;
        set
        {
            _selectedRiskFilter = value ?? string.Empty;
            OnPropertyChanged();
            RefreshTweakList();
        }
    }

    private string _selectedCategoryFilter = string.Empty;
    public string SelectedCategoryFilter
    {
        get => _selectedCategoryFilter;
        set
        {
            _selectedCategoryFilter = value ?? string.Empty;
            OnPropertyChanged();
            RefreshTweakList();
        }
    }

    private string _logSearchText = string.Empty;
    public string LogSearchText
    {
        get => _logSearchText;
        set
        {
            _logSearchText = value ?? string.Empty;
            OnPropertyChanged();
            ApplyLogFilters();
        }
    }

    private string _selectedLogStatusFilter = string.Empty;
    public string SelectedLogStatusFilter
    {
        get => _selectedLogStatusFilter;
        set
        {
            _selectedLogStatusFilter = value ?? string.Empty;
            OnPropertyChanged();
            ApplyLogFilters();
        }
    }

    private string _selectedLogTypeFilter = string.Empty;
    public string SelectedLogTypeFilter
    {
        get => _selectedLogTypeFilter;
        set
        {
            _selectedLogTypeFilter = value ?? string.Empty;
            OnPropertyChanged();
            ApplyLogFilters();
        }
    }

    private int _selectedLogPageSize = 50;
    public int SelectedLogPageSize
    {
        get => _selectedLogPageSize;
        set
        {
            _selectedLogPageSize = value <= 0 ? 50 : value;
            OnPropertyChanged();
            _currentLogPage = 1;
            ApplyLogFilters();
        }
    }

    private int _currentLogPage = 1;
    public int CurrentLogPage
    {
        get => _currentLogPage;
        private set
        {
            _currentLogPage = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(LogPageSummary));
        }
    }

    private int _totalLogPages = 1;
    public int TotalLogPages
    {
        get => _totalLogPages;
        private set
        {
            _totalLogPages = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(LogPageSummary));
        }
    }

    public string LogPageSummary => $"Page {CurrentLogPage} / {TotalLogPages}";

    private string _monitoringStatus = "Monitoreo pausado";
    public string MonitoringStatus
    {
        get => _monitoringStatus;
        private set
        {
            _monitoringStatus = value;
            OnPropertyChanged();
        }
    }

    private string _monitoringTimestamp = string.Empty;
    public string MonitoringTimestamp
    {
        get => _monitoringTimestamp;
        private set
        {
            _monitoringTimestamp = value;
            OnPropertyChanged();
        }
    }

    private bool _isMonitoringEnabled = true;
    public bool IsMonitoringEnabled
    {
        get => _isMonitoringEnabled;
        set
        {
            if (_isMonitoringEnabled == value)
            {
                return;
            }
            _isMonitoringEnabled = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(MonitoringToggleLabel));
            PersistMonitoringState();
            UpdateMonitoringState();
        }
    }

    public string MonitoringToggleLabel => IsMonitoringEnabled ? "Pausar" : "Reanudar";

    private bool _metricsLoggingEnabled;
    public bool MetricsLoggingEnabled
    {
        get => _metricsLoggingEnabled;
        set
        {
            if (_metricsLoggingEnabled == value)
            {
                return;
            }
            _metricsLoggingEnabled = value;
            OnPropertyChanged();
            PersistMonitoringState();
        }
    }

    private bool _createRestorePointBeforeApply = true;
    public bool CreateRestorePointBeforeApply
    {
        get => _createRestorePointBeforeApply;
        set
        {
            if (_createRestorePointBeforeApply == value)
            {
                return;
            }
            _createRestorePointBeforeApply = value;
            OnPropertyChanged();
            PersistRestorePointSetting();
        }
    }

    private double _cpuUsagePercent;
    public double CpuUsagePercent
    {
        get => _cpuUsagePercent;
        private set
        {
            _cpuUsagePercent = value;
            OnPropertyChanged();
        }
    }

    private string _cpuUsageLabel = "0%";
    public string CpuUsageLabel
    {
        get => _cpuUsageLabel;
        private set
        {
            _cpuUsageLabel = value;
            OnPropertyChanged();
        }
    }

    private string _cpuTempLabel = "N/A";
    public string CpuTempLabel
    {
        get => _cpuTempLabel;
        private set
        {
            _cpuTempLabel = value;
            OnPropertyChanged();
        }
    }

    private double _gpuUsagePercent;
    public double GpuUsagePercent
    {
        get => _gpuUsagePercent;
        private set
        {
            _gpuUsagePercent = value;
            OnPropertyChanged();
        }
    }

    private string _gpuUsageLabel = "N/A";
    public string GpuUsageLabel
    {
        get => _gpuUsageLabel;
        private set
        {
            _gpuUsageLabel = value;
            OnPropertyChanged();
        }
    }

    private string _gpuNameLabel = "GPU";
    public string GpuNameLabel
    {
        get => _gpuNameLabel;
        private set
        {
            _gpuNameLabel = value;
            OnPropertyChanged();
        }
    }

    private string _gpuVramLabel = string.Empty;
    public string GpuVramLabel
    {
        get => _gpuVramLabel;
        private set
        {
            _gpuVramLabel = value;
            OnPropertyChanged();
        }
    }

    private double _memoryUsagePercent;
    public double MemoryUsagePercent
    {
        get => _memoryUsagePercent;
        private set
        {
            _memoryUsagePercent = value;
            OnPropertyChanged();
        }
    }

    private string _memoryUsageLabel = "0 / 0 GB";
    public string MemoryUsageLabel
    {
        get => _memoryUsageLabel;
        private set
        {
            _memoryUsageLabel = value;
            OnPropertyChanged();
        }
    }

    private double _diskUsagePercent;
    public double DiskUsagePercent
    {
        get => _diskUsagePercent;
        private set
        {
            _diskUsagePercent = value;
            OnPropertyChanged();
        }
    }

    private string _diskUsageLabel = "Disk";
    public string DiskUsageLabel
    {
        get => _diskUsageLabel;
        private set
        {
            _diskUsageLabel = value;
            OnPropertyChanged();
        }
    }

    private string _networkUsageLabel = "Descarga 0 Mbps / Subida 0 Mbps";
    public string NetworkUsageLabel
    {
        get => _networkUsageLabel;
        private set
        {
            _networkUsageLabel = value;
            OnPropertyChanged();
        }
    }

    private string _benchmarkStatus = "Benchmark listo";
    public string BenchmarkStatus
    {
        get => _benchmarkStatus;
        private set
        {
            _benchmarkStatus = value;
            OnPropertyChanged();
        }
    }

    private string _benchmarkProgressLabel = "Listo";
    public string BenchmarkProgressLabel
    {
        get => _benchmarkProgressLabel;
        private set
        {
            _benchmarkProgressLabel = value;
            OnPropertyChanged();
        }
    }

    private double _benchmarkProgressPercent;
    public double BenchmarkProgressPercent
    {
        get => _benchmarkProgressPercent;
        private set
        {
            _benchmarkProgressPercent = value;
            OnPropertyChanged();
        }
    }

    private bool _isBenchmarkRunning;
    public bool IsBenchmarkRunning
    {
        get => _isBenchmarkRunning;
        private set
        {
            _isBenchmarkRunning = value;
            OnPropertyChanged();
            _runQuickBenchmarkCommand?.RaiseCanExecuteChanged();
            _runFullBenchmarkCommand?.RaiseCanExecuteChanged();
            _compareBenchmarksCommand?.RaiseCanExecuteChanged();
            _exportComparisonReportCommand?.RaiseCanExecuteChanged();
        }
    }

    private BenchmarkRunItem? _selectedBenchmarkBefore;
    public BenchmarkRunItem? SelectedBenchmarkBefore
    {
        get => _selectedBenchmarkBefore;
        set
        {
            _selectedBenchmarkBefore = value;
            OnPropertyChanged();
            _compareBenchmarksCommand?.RaiseCanExecuteChanged();
            _exportComparisonReportCommand?.RaiseCanExecuteChanged();
        }
    }

    private BenchmarkRunItem? _selectedBenchmarkAfter;
    public BenchmarkRunItem? SelectedBenchmarkAfter
    {
        get => _selectedBenchmarkAfter;
        set
        {
            _selectedBenchmarkAfter = value;
            OnPropertyChanged();
            _compareBenchmarksCommand?.RaiseCanExecuteChanged();
            _exportComparisonReportCommand?.RaiseCanExecuteChanged();
        }
    }

    private string _benchmarkComparisonSummary = "Selecciona dos runs para comparar.";
    public string BenchmarkComparisonSummary
    {
        get => _benchmarkComparisonSummary;
        private set
        {
            _benchmarkComparisonSummary = value;
            OnPropertyChanged();
        }
    }

    private readonly ExecutionEngine _engine;
    private readonly ExecutionLogger _logger;
    private readonly HardwareDetector _hardwareDetector = new();
    private readonly HardwareRuleEvaluator _hardwareRuleEvaluator = new();
    private readonly SystemMetricsService _metricsService = new();
    private readonly BenchmarkRunner _benchmarkRunner = new();
    private readonly BenchmarkStorage _benchmarkStorage = new();
    private SystemMetricsLogger _metricsLogger = new("");
    private Timer? _monitorTimer;
    private int _monitorIntervalMs = 2000;
    private string _benchmarkResultsPath = string.Empty;
    private readonly UserSettingsService _settingsService;
    private UserSettings _settings = new();
    private readonly SynchronizationContext _uiContext;
    private ConfigBundle? _bundle;
    private Dictionary<string, TweakDefinition> _tweakMap = new(StringComparer.OrdinalIgnoreCase);
    private Dictionary<string, ServiceDefinition> _serviceMap = new(StringComparer.OrdinalIgnoreCase);
    private Dictionary<string, AppPackageDefinition> _appMap = new(StringComparer.OrdinalIgnoreCase);

    public int TweakCount { get; private set; }
    public int ServiceCount { get; private set; }
    public int AppCount { get; private set; }

    private string _configStatus = "Config loaded";
    public string ConfigStatus
    {
        get => _configStatus;
        private set
        {
            _configStatus = value;
            OnPropertyChanged();
        }
    }

    private string _lastActionStatus = "Ready";
    public string LastActionStatus
    {
        get => _lastActionStatus;
        private set
        {
            _lastActionStatus = value;
            OnPropertyChanged();
        }
    }

    public string LogPath { get; }

    private string _hardwareStatus = "Hardware not detected";
    public string HardwareStatus
    {
        get => _hardwareStatus;
        private set
        {
            _hardwareStatus = value;
            OnPropertyChanged();
        }
    }

    private string _hardwareSummary = "";
    public string HardwareSummary
    {
        get => _hardwareSummary;
        private set
        {
            _hardwareSummary = value;
            OnPropertyChanged();
        }
    }

    private string _recommendationsStatus = "No recommendations";
    public string RecommendationsStatus
    {
        get => _recommendationsStatus;
        private set
        {
            _recommendationsStatus = value;
            OnPropertyChanged();
        }
    }

    private string _logStatusSummary = "No logs";
    public string LogStatusSummary
    {
        get => _logStatusSummary;
        private set
        {
            _logStatusSummary = value;
            OnPropertyChanged();
        }
    }

    private string _selectedVrrOverride = string.Empty;
    public string SelectedVrrOverride
    {
        get => _selectedVrrOverride;
        set
        {
            _selectedVrrOverride = value ?? string.Empty;
            OnPropertyChanged();
            ApplyVrrOverrideSelection();
        }
    }

    private string _gpuNameOverride = string.Empty;
    public string GpuNameOverride
    {
        get => _gpuNameOverride;
        set
        {
            _gpuNameOverride = value ?? string.Empty;
            OnPropertyChanged();
            ApplyGpuOverrideSelection();
        }
    }

    private string _gpuVendorOverride = string.Empty;
    public string GpuVendorOverride
    {
        get => _gpuVendorOverride;
        set
        {
            _gpuVendorOverride = value ?? string.Empty;
            OnPropertyChanged();
            ApplyGpuOverrideSelection();
        }
    }

    private string _cpuNameOverride = string.Empty;
    public string CpuNameOverride
    {
        get => _cpuNameOverride;
        set
        {
            _cpuNameOverride = value ?? string.Empty;
            OnPropertyChanged();
            ApplyCpuRamOverrideSelection();
        }
    }

    private string _cpuCoresOverride = string.Empty;
    public string CpuCoresOverride
    {
        get => _cpuCoresOverride;
        set
        {
            _cpuCoresOverride = value ?? string.Empty;
            OnPropertyChanged();
            ApplyCpuRamOverrideSelection();
        }
    }

    private string _cpuThreadsOverride = string.Empty;
    public string CpuThreadsOverride
    {
        get => _cpuThreadsOverride;
        set
        {
            _cpuThreadsOverride = value ?? string.Empty;
            OnPropertyChanged();
            ApplyCpuRamOverrideSelection();
        }
    }

    private string _ramTotalGbOverride = string.Empty;
    public string RamTotalGbOverride
    {
        get => _ramTotalGbOverride;
        set
        {
            _ramTotalGbOverride = value ?? string.Empty;
            OnPropertyChanged();
            ApplyCpuRamOverrideSelection();
        }
    }

    private string _ramSpeedOverride = string.Empty;
    public string RamSpeedOverride
    {
        get => _ramSpeedOverride;
        set
        {
            _ramSpeedOverride = value ?? string.Empty;
            OnPropertyChanged();
            ApplyCpuRamOverrideSelection();
        }
    }

    private string _osCaptionOverride = string.Empty;
    public string OsCaptionOverride
    {
        get => _osCaptionOverride;
        set
        {
            _osCaptionOverride = value ?? string.Empty;
            OnPropertyChanged();
            ApplyOsOverrideSelection();
        }
    }

    private string _osVersionOverride = string.Empty;
    public string OsVersionOverride
    {
        get => _osVersionOverride;
        set
        {
            _osVersionOverride = value ?? string.Empty;
            OnPropertyChanged();
            ApplyOsOverrideSelection();
        }
    }

    private string _osBuildOverride = string.Empty;
    public string OsBuildOverride
    {
        get => _osBuildOverride;
        set
        {
            _osBuildOverride = value ?? string.Empty;
            OnPropertyChanged();
            ApplyOsOverrideSelection();
        }
    }

    private readonly RelayCommand _applySelectedProfileCommand;
    public ICommand ApplySelectedProfileCommand => _applySelectedProfileCommand;

    private readonly RelayCommand _revertSelectedProfileCommand;
    public ICommand RevertSelectedProfileCommand => _revertSelectedProfileCommand;

    private readonly RelayCommand _applySelectedTweakCommand;
    public ICommand ApplySelectedTweakCommand => _applySelectedTweakCommand;

    private readonly RelayCommand _revertSelectedTweakCommand;
    public ICommand RevertSelectedTweakCommand => _revertSelectedTweakCommand;

    private readonly RelayCommand _revertProfileCatalogCommand;
    public ICommand RevertProfileCatalogCommand => _revertProfileCatalogCommand;

    private readonly RelayCommand _refreshLogsCommand;
    public ICommand RefreshLogsCommand => _refreshLogsCommand;

    private readonly RelayCommand _exportLogsCommand;
    public ICommand ExportLogsCommand => _exportLogsCommand;

    private readonly RelayCommand _exportPageCommand;
    public ICommand ExportPageCommand => _exportPageCommand;

    private readonly RelayCommand _openLogFolderCommand;
    public ICommand OpenLogFolderCommand => _openLogFolderCommand;

    private readonly RelayCommand _resetHardwareOverridesCommand;
    public ICommand ResetHardwareOverridesCommand => _resetHardwareOverridesCommand;

    private readonly RelayCommand _toggleMonitoringCommand;
    public ICommand ToggleMonitoringCommand => _toggleMonitoringCommand;

    private readonly RelayCommand _runQuickBenchmarkCommand;
    public ICommand RunQuickBenchmarkCommand => _runQuickBenchmarkCommand;

    private readonly RelayCommand _runFullBenchmarkCommand;
    public ICommand RunFullBenchmarkCommand => _runFullBenchmarkCommand;

    private readonly RelayCommand _refreshBenchmarkRunsCommand;
    public ICommand RefreshBenchmarkRunsCommand => _refreshBenchmarkRunsCommand;

    private readonly RelayCommand _compareBenchmarksCommand;
    public ICommand CompareBenchmarksCommand => _compareBenchmarksCommand;

    private readonly RelayCommand _exportComparisonReportCommand;
    public ICommand ExportComparisonReportCommand => _exportComparisonReportCommand;

    private readonly RelayCommand _openBenchmarkFolderCommand;
    public ICommand OpenBenchmarkFolderCommand => _openBenchmarkFolderCommand;

    private readonly RelayCommand _nextLogPageCommand;
    public ICommand NextLogPageCommand => _nextLogPageCommand;

    private readonly RelayCommand _prevLogPageCommand;
    public ICommand PrevLogPageCommand => _prevLogPageCommand;

    private readonly Dictionary<string, string> _lastProfileRunIds = new(StringComparer.OrdinalIgnoreCase);
    private bool _suppressOverrideUpdates;

    public MainViewModel()
    {
        _uiContext = SynchronizationContext.Current ?? new SynchronizationContext();
        LogPath = ResolveLogPath();
        _settingsService = new UserSettingsService(ResolveSettingsPath());
        _settings = _settingsService.Load();
        _metricsLogger = new SystemMetricsLogger(ResolveMetricsLogPath());
        _benchmarkResultsPath = ResolveBenchmarkResultsPath();
        try
        {
            _logger = new ExecutionLogger(LogPath);
            _engine = new ExecutionEngine(_logger);
        }
        catch (Exception ex)
        {
            LogPath = Path.Combine(Path.GetTempPath(), "ljokerl_execution.jsonl");
            _logger = new ExecutionLogger(LogPath);
            _engine = new ExecutionEngine(_logger);
            ConfigStatus = $"Init error: {ex.Message}";
            LastActionStatus = "Init failed; using fallback log path";
        }
        _applySelectedProfileCommand = new RelayCommand(_ => ApplySelectedProfile(), _ => CanApplyProfile());
        _revertSelectedProfileCommand = new RelayCommand(_ => RevertSelectedProfile(), _ => CanApplyProfile());
        _applySelectedTweakCommand = new RelayCommand(_ => ApplySelectedTweak(), _ => SelectedTweak != null);
        _revertSelectedTweakCommand = new RelayCommand(_ => RevertSelectedTweak(), _ => SelectedTweak != null);
        _revertProfileCatalogCommand = new RelayCommand(_ => RevertSelectedProfileCatalog(), _ => CanApplyProfile());
        _refreshLogsCommand = new RelayCommand(_ => RefreshLogs());
        _exportLogsCommand = new RelayCommand(_ => ExportLogs());
        _exportPageCommand = new RelayCommand(_ => ExportCurrentPage());
        _openLogFolderCommand = new RelayCommand(_ => OpenLogFolder());
        _nextLogPageCommand = new RelayCommand(_ => NextLogPage(), _ => CurrentLogPage < TotalLogPages);
        _prevLogPageCommand = new RelayCommand(_ => PrevLogPage(), _ => CurrentLogPage > 1);
        _resetHardwareOverridesCommand = new RelayCommand(_ => ResetHardwareOverrides());
        _toggleMonitoringCommand = new RelayCommand(_ => ToggleMonitoring());
        _runQuickBenchmarkCommand = new RelayCommand(_ => RunBenchmarkAsync(BenchmarkRunOptions.CreateQuick()), _ => !IsBenchmarkRunning);
        _runFullBenchmarkCommand = new RelayCommand(_ => RunBenchmarkAsync(new BenchmarkRunOptions()), _ => !IsBenchmarkRunning);
        _refreshBenchmarkRunsCommand = new RelayCommand(_ => RefreshBenchmarkRuns());
        _compareBenchmarksCommand = new RelayCommand(_ => CompareBenchmarks(), _ => CanCompareBenchmarks());
        _exportComparisonReportCommand = new RelayCommand(_ => ExportComparisonReport(), _ => CanCompareBenchmarks());
        _openBenchmarkFolderCommand = new RelayCommand(_ => OpenBenchmarkFolder());
        InitializeFilters();
        InitializeVrrOptions();
        InitializeGpuOverrides();
        InitializeCpuRamOverrides();
        InitializeOsOverrides();
        InitializeLogFilters();
        InitializeMonitoring();
        RefreshBenchmarkRuns();
        RefreshLogs();
        LoadConfigAsync();
    }

    private async void LoadConfigAsync()
    {
        try
        {
            var configRoot = ResolveConfigRoot();
            if (!Directory.Exists(configRoot))
            {
                ConfigStatus = $"Config directory not found: {configRoot}";
                LastActionStatus = "Config load failed";
                return;
            }
            var loader = new ConfigLoader();
            var bundle = await Task.Run(() => loader.LoadAll(configRoot));
            _uiContext.Post(_ => ApplyConfig(bundle), null);
        }
        catch (Exception ex)
        {
            ConfigStatus = $"Config error: {ex.Message}";
            LastActionStatus = "Config load failed";
        }
    }

    private void ApplyConfig(ConfigBundle bundle)
    {
        _bundle = bundle;
        if (bundle.Errors.Count > 0)
        {
            ConfigStatus = $"Config loaded with warnings: {string.Join(" | ", bundle.Errors)}";
        }
        else
        {
            ConfigStatus = "Config loaded";
        }

        _tweakMap = bundle.Tweaks?.Tweaks.ToDictionary(t => t.Id, StringComparer.OrdinalIgnoreCase)
            ?? new(StringComparer.OrdinalIgnoreCase);
        _serviceMap = bundle.Services?.Services.ToDictionary(s => s.Id, StringComparer.OrdinalIgnoreCase)
            ?? new(StringComparer.OrdinalIgnoreCase);
        _appMap = bundle.Apps?.Apps.ToDictionary(a => a.Id, StringComparer.OrdinalIgnoreCase)
            ?? new(StringComparer.OrdinalIgnoreCase);

        _allTweaks.Clear();
        foreach (var tweak in bundle.Tweaks?.Tweaks ?? Enumerable.Empty<TweakDefinition>())
        {
            _allTweaks.Add(new TweakItem(tweak));
        }
        RefreshCategoryFilters();
        RefreshTweakList();

        TweakCount = bundle.Tweaks?.Tweaks.Count ?? 0;
        ServiceCount = bundle.Services?.Services.Count ?? 0;
        AppCount = bundle.Apps?.Apps.Count ?? 0;

        Profiles.Clear();
        foreach (var profile in bundle.Profiles?.Profiles ?? Enumerable.Empty<ProfileDefinition>())
        {
            var tweakTotal = profile.Tweaks.Count(id => _tweakMap.ContainsKey(id));
            var serviceTotal = profile.Services.Count(id => _serviceMap.ContainsKey(id));
            var appTotal = profile.Apps.Count(id => _appMap.ContainsKey(id));

            Profiles.Add(new ProfileItem(profile, tweakTotal, serviceTotal, appTotal));
        }

        OnPropertyChanged(nameof(TweakCount));
        OnPropertyChanged(nameof(ServiceCount));
        OnPropertyChanged(nameof(AppCount));
        _applySelectedProfileCommand.RaiseCanExecuteChanged();
        _revertSelectedProfileCommand.RaiseCanExecuteChanged();
        _revertProfileCatalogCommand.RaiseCanExecuteChanged();

        LoadHardwareAsync(bundle.HardwareRules, _settings.VrrOverride);
    }

    private async void LoadHardwareAsync(HardwareRuleCatalog? rules, bool? vrrOverride)
    {
        if (rules == null)
        {
            HardwareStatus = "Hardware rules not loaded";
            return;
        }

        HardwareStatus = "Detecting hardware...";
        try
        {
            var snapshot = await Task.Run(() => _hardwareDetector.Detect(vrrOverride));
            ApplyCpuOverrides(snapshot);
            ApplyMemoryOverrides(snapshot);
            ApplyOsOverrides(snapshot);
            ApplyGpuOverrides(snapshot);
            var recommendation = _hardwareRuleEvaluator.Evaluate(snapshot, rules);
            _uiContext.Post(_ => ApplyHardware(snapshot, recommendation), null);
        }
        catch (Exception ex)
        {
            HardwareStatus = $"Hardware detection failed: {ex.Message}";
        }
    }

    private void ApplyHardware(HardwareSnapshot snapshot, HardwareRecommendationResult recommendation)
    {
        HardwareStatus = "Hardware detected";
        HardwareSummary = BuildHardwareSummary(snapshot);
        UpdateHardwareFlags(snapshot, recommendation);

        Recommendations.Clear();
        foreach (var match in recommendation.Matches)
        {
            Recommendations.Add(new RecommendationItem(match.RuleId, FormatRecommendation(match.Recommendation)));
        }

        if (Recommendations.Count == 0)
        {
            RecommendationsStatus = recommendation.Warnings.Count > 0
                ? string.Join(" | ", recommendation.Warnings)
                : "No recommendations";
        }
        else
        {
            RecommendationsStatus = $"{Recommendations.Count} recommendation(s)";
        }
    }

    private static string BuildHardwareSummary(HardwareSnapshot snapshot)
    {
        var lines = new List<string>();
        if (snapshot.Cpu != null)
        {
            var cores = snapshot.Cpu.Cores?.ToString() ?? "?";
            var threads = snapshot.Cpu.LogicalProcessors?.ToString() ?? "?";
            lines.Add($"CPU: {snapshot.Cpu.Name} ({cores}C/{threads}T)");
        }
        else
        {
            lines.Add("CPU: Unknown");
        }

        if (snapshot.Gpu != null)
        {
            var vram = snapshot.Gpu.VramBytes.HasValue ? $"{snapshot.Gpu.VramBytes.Value / (1024 * 1024)} MB" : "?";
            var vendor = string.IsNullOrWhiteSpace(snapshot.Gpu.Vendor) ? "" : $" - {snapshot.Gpu.Vendor}";
            lines.Add($"GPU: {snapshot.Gpu.Name}{vendor} ({vram})");
        }
        else
        {
            lines.Add("GPU: Unknown");
        }

        if (snapshot.Memory != null)
        {
            var ram = snapshot.Memory.TotalBytes.HasValue ? $"{snapshot.Memory.TotalBytes.Value / (1024 * 1024 * 1024)} GB" : "?";
            lines.Add($"RAM: {ram} @ {snapshot.Memory.AverageSpeedMhz ?? 0} MHz");
        }
        else
        {
            lines.Add("RAM: Unknown");
        }

        if (snapshot.Os != null)
        {
            lines.Add($"OS: {snapshot.Os.Caption} ({snapshot.Os.Version} build {snapshot.Os.BuildNumber})");
        }
        else
        {
            lines.Add("OS: Unknown");
        }

        var vrr = snapshot.Monitor?.VrrSupported == true ? "Yes" : snapshot.Monitor?.VrrSupported == false ? "No" : "Unknown";
        lines.Add($"VRR: {vrr}");

        return string.Join(Environment.NewLine, lines);
    }

    private void UpdateHardwareFlags(HardwareSnapshot snapshot, HardwareRecommendationResult recommendation)
    {
        HardwareFlags.Clear();

        var cpuCores = snapshot.Cpu?.Cores?.ToString() ?? "?";
        var cpuThreads = snapshot.Cpu?.LogicalProcessors?.ToString() ?? "?";
        var cpuSource = HasCpuOverride() ? "Manual (UI)" : "Detected";
        HardwareFlags.Add(new HardwareFlagItem("CPU", $"{cpuCores}C/{cpuThreads}T", cpuSource));

        var gpuVendor = snapshot.Gpu?.Vendor ?? "Unknown";
        var gpuVram = snapshot.Gpu?.VramBytes.HasValue == true
            ? $"{snapshot.Gpu.VramBytes.Value / (1024 * 1024)} MB"
            : "?";
        var gpuSource = HasGpuOverride() ? "Manual (UI)" : "Detected";
        HardwareFlags.Add(new HardwareFlagItem("GPU", $"{gpuVendor} ({gpuVram})", gpuSource));

        var ramSource = HasRamOverride() ? "Manual (UI)" : "Detected";
        var ramValue = snapshot.Memory?.TotalBytes.HasValue == true
            ? $"{snapshot.Memory.TotalBytes.Value / (1024 * 1024 * 1024)} GB"
            : "?";
        HardwareFlags.Add(new HardwareFlagItem("RAM", ramValue, ramSource));

        var vrrValue = snapshot.Monitor?.VrrSupported == true ? "On" : snapshot.Monitor?.VrrSupported == false ? "Off" : "Unknown";
        var vrrSource = GetVrrSource();
        HardwareFlags.Add(new HardwareFlagItem("VRR", vrrValue, vrrSource));

        var osSource = HasOsOverride() ? "Manual (UI)" : "Detected";
        var osLabel = snapshot.Os?.Caption ?? "Unknown";
        HardwareFlags.Add(new HardwareFlagItem("OS", osLabel, osSource));

        HardwareFlags.Add(new HardwareFlagItem("Rules", $"{recommendation.Matches.Count} matched", "hardware_rules.json"));
    }

    private string GetVrrSource()
    {
        if (_settings.VrrOverride.HasValue)
        {
            return "Manual (UI)";
        }

        var envOverride = ParseVrrOverride(Environment.GetEnvironmentVariable("LJOKERL_VRR_OVERRIDE"));
        if (envOverride.HasValue)
        {
            return "Env Override";
        }

        return "Auto";
    }

    private bool HasGpuOverride()
    {
        return !string.IsNullOrWhiteSpace(_settings.GpuNameOverride) ||
               !string.IsNullOrWhiteSpace(_settings.GpuVendorOverride);
    }

    private bool HasCpuOverride()
    {
        return !string.IsNullOrWhiteSpace(_settings.CpuNameOverride) ||
               _settings.CpuCoresOverride.HasValue ||
               _settings.CpuThreadsOverride.HasValue;
    }

    private bool HasRamOverride()
    {
        return _settings.RamTotalGbOverride.HasValue || _settings.RamSpeedMhzOverride.HasValue;
    }

    private bool HasOsOverride()
    {
        return !string.IsNullOrWhiteSpace(_settings.OsCaptionOverride) ||
               !string.IsNullOrWhiteSpace(_settings.OsVersionOverride) ||
               !string.IsNullOrWhiteSpace(_settings.OsBuildOverride);
    }

    private static bool? ParseVrrOverride(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return null;
        }

        var value = raw.Trim().ToLowerInvariant();
        if (value is "1" or "true" or "yes" or "on")
        {
            return true;
        }

        if (value is "0" or "false" or "no" or "off")
        {
            return false;
        }

        return null;
    }

    private static string FormatRecommendation(HardwareRuleRecommendation recommendation)
    {
        var parts = new List<string>();
        if (recommendation.FpsCapOffset.HasValue)
        {
            parts.Add($"fpsCapOffset {recommendation.FpsCapOffset.Value}");
        }

        if (recommendation.VsyncDriver.HasValue)
        {
            parts.Add($"vsyncDriver {(recommendation.VsyncDriver.Value ? "on" : "off")}");
        }

        if (recommendation.VsyncInGame.HasValue)
        {
            parts.Add($"vsyncInGame {(recommendation.VsyncInGame.Value ? "on" : "off")}");
        }

        return parts.Count == 0 ? "No specific settings" : string.Join(" | ", parts);
    }

    private bool CanApplyProfile()
    {
        return _bundle != null && SelectedProfile != null;
    }

    private void ApplySelectedProfile()
    {
        if (_bundle == null || SelectedProfile == null)
        {
            LastActionStatus = "No profile selected";
            return;
        }

        if (!IsAdministrator())
        {
            LastActionStatus = "Requiere permisos de administrador";
            return;
        }

        var build = OsVersionCheck.CurrentBuild();
        var support = OsVersionCheck.Decide(build);
        if (support == WindowsBuildSupport.Unsupported)
        {
            LastActionStatus = $"Build de Windows no soportada ({build}). Se requiere Windows 10 19041+ o Windows 11.";
            return;
        }

        var osWarning = support == WindowsBuildSupport.SupportedLegacy
            ? "AVISO: build de Windows 10 sin validación oficial; el resultado no está garantizado. "
            : string.Empty;

        var restoreNote = string.Empty;
        if (_settings.CreateRestorePointBeforeApply)
        {
            var restoreService = new SystemRestoreService();
            var systemRestoreEnabled = restoreService.IsEnabled();
            var lastPointUtc = systemRestoreEnabled ? restoreService.GetLastRestorePointUtc() : null;
            var hasRecentPoint = lastPointUtc.HasValue &&
                                 DateTime.UtcNow - lastPointUtc.Value < SystemRestoreGate.CreationCooldown;

            var creationSucceeded = hasRecentPoint;
            if (systemRestoreEnabled && !hasRecentPoint)
            {
                creationSucceeded = restoreService.TryCreate($"LJokerL_TTV {SelectedProfile.Name}", out _);
            }

            var gate = SystemRestoreGate.Decide(true, systemRestoreEnabled, lastPointUtc, DateTime.UtcNow, creationSucceeded);
            if (gate.Decision == SystemRestoreDecision.SkipFailSafe)
            {
                LastActionStatus = gate.Message;
                return;
            }

            restoreNote = gate.Message;
        }
        var errors = new List<string>();
        var actionCount = 0;
        var contextId = CreateProfileContextId(SelectedProfile.Id);
        _lastProfileRunIds[SelectedProfile.Id] = contextId;

        foreach (var tweakId in SelectedProfile.TweakIds)
        {
            if (_tweakMap.TryGetValue(tweakId, out var tweak))
            {
                var report = _engine.ApplyTweak(tweak, contextId);
                actionCount += report.Actions.Count;
                if (!report.Success)
                {
                    errors.Add($"tweak:{tweakId}");
                }
            }
            else
            {
                errors.Add($"tweak:{tweakId}(missing)");
            }
        }

        foreach (var serviceId in SelectedProfile.ServiceIds)
        {
            if (_serviceMap.TryGetValue(serviceId, out var service))
            {
                var report = _engine.ApplyService(service, contextId);
                actionCount += report.Actions.Count;
                if (!report.Success)
                {
                    errors.Add($"service:{serviceId}");
                }
            }
            else
            {
                errors.Add($"service:{serviceId}(missing)");
            }
        }

        foreach (var appId in SelectedProfile.AppIds)
        {
            if (_appMap.TryGetValue(appId, out var app))
            {
                var report = _engine.RemoveApp(app, contextId);
                actionCount += report.Actions.Count;
                if (!report.Success)
                {
                    errors.Add($"app:{appId}");
                }
            }
            else
            {
                errors.Add($"app:{appId}(missing)");
            }
        }

        if (errors.Count == 0)
        {
            LastActionStatus = $"{osWarning}Perfil aplicado: {SelectedProfile.Name}. Acciones: {actionCount}. Log: {LogPath}" +
                               (restoreNote.Length > 0 ? $" {restoreNote}" : string.Empty);
        }
        else
        {
            LastActionStatus = $"Perfil aplicado con errores ({errors.Count}). Ver log: {LogPath}" +
                               (restoreNote.Length > 0 ? $" {restoreNote}" : string.Empty);
        }

        RefreshLogs();
    }

    private void RevertSelectedProfile()
    {
        if (_bundle == null || SelectedProfile == null)
        {
            LastActionStatus = "No profile selected";
            return;
        }

        if (!IsAdministrator())
        {
            LastActionStatus = "Requiere permisos de administrador";
            return;
        }

        var profileId = SelectedProfile.Id;
        if (!_lastProfileRunIds.TryGetValue(profileId, out var contextId))
        {
            contextId = _engine.FindLatestContextForProfile(profileId);
        }

        if (string.IsNullOrWhiteSpace(contextId))
        {
            LastActionStatus = "No se encontro un log para revertir este perfil";
            return;
        }

        var rollback = _engine.RollbackContext(contextId);
        LastActionStatus = $"Rollback log perfil {SelectedProfile.Name}: restaurados {rollback.RestoredCount}, omitidos {rollback.SkippedCount}, fallidos {rollback.FailedCount}.";
    }

    private void RevertSelectedProfileCatalog()
    {
        if (_bundle == null || SelectedProfile == null)
        {
            LastActionStatus = "No profile selected";
            return;
        }

        if (!IsAdministrator())
        {
            LastActionStatus = "Requiere permisos de administrador";
            return;
        }

        var errors = new List<string>();
        var actionCount = 0;
        var contextId = $"profile:{SelectedProfile.Id}:revert:{DateTime.UtcNow:yyyyMMddHHmmss}";

        foreach (var tweakId in SelectedProfile.TweakIds)
        {
            if (_tweakMap.TryGetValue(tweakId, out var tweak))
            {
                var report = _engine.RevertTweak(tweak, contextId);
                actionCount += report.Actions.Count;
                if (!report.Success)
                {
                    errors.Add($"tweak:{tweakId}");
                }
            }
        }

        foreach (var serviceId in SelectedProfile.ServiceIds)
        {
            if (_serviceMap.TryGetValue(serviceId, out var service))
            {
                var report = _engine.RevertService(service, contextId);
                actionCount += report.Actions.Count;
                if (!report.Success)
                {
                    errors.Add($"service:{serviceId}");
                }
            }
        }

        foreach (var appId in SelectedProfile.AppIds)
        {
            if (_appMap.TryGetValue(appId, out var app))
            {
                var report = _engine.RestoreApp(app, contextId);
                actionCount += report.Actions.Count;
                if (!report.Success)
                {
                    errors.Add($"app:{appId}");
                }
            }
        }

        if (errors.Count == 0)
        {
            LastActionStatus = $"Perfil revertido (catalogo): {SelectedProfile.Name}. Acciones: {actionCount}.";
        }
        else
        {
            LastActionStatus = $"Perfil revertido con errores ({errors.Count}). Ver log: {LogPath}";
        }

        RefreshLogs();
    }

    private void ApplySelectedTweak()
    {
        if (SelectedTweak == null)
        {
            LastActionStatus = "No tweak selected";
            return;
        }

        if (!IsAdministrator())
        {
            LastActionStatus = "Requiere permisos de administrador";
            return;
        }

        var report = _engine.ApplyTweak(SelectedTweak.Definition, $"tweak:{SelectedTweak.Id}:{DateTime.UtcNow:yyyyMMddHHmmss}");
        LastActionStatus = report.Success
            ? $"Tweak aplicado: {SelectedTweak.Id}. Log: {LogPath}"
            : $"Tweak aplicado con errores: {SelectedTweak.Id}. Ver log: {LogPath}";

        RefreshLogs();
    }

    private void RevertSelectedTweak()
    {
        if (SelectedTweak == null)
        {
            LastActionStatus = "No tweak selected";
            return;
        }

        if (!IsAdministrator())
        {
            LastActionStatus = "Requiere permisos de administrador";
            return;
        }

        var report = _engine.RevertTweak(SelectedTweak.Definition, $"tweak:{SelectedTweak.Id}:{DateTime.UtcNow:yyyyMMddHHmmss}");
        LastActionStatus = report.Success
            ? $"Tweak revertido: {SelectedTweak.Id}. Log: {LogPath}"
            : $"Tweak revertido con errores: {SelectedTweak.Id}. Ver log: {LogPath}";

        RefreshLogs();
    }

    private void RefreshTweakList()
    {
        AdvancedTweaks.Clear();
        var search = _tweakSearchText.Trim();
        var riskFilter = _selectedRiskFilter.Trim();
        var categoryFilter = _selectedCategoryFilter.Trim();

        IEnumerable<TweakItem> filtered = _allTweaks;
        if (!string.IsNullOrWhiteSpace(search))
        {
            filtered = filtered.Where(t =>
                t.Id.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                t.Description.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                t.Category.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                t.Risk.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(riskFilter))
        {
            filtered = filtered.Where(t => string.Equals(t.Risk, riskFilter, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(categoryFilter))
        {
            filtered = filtered.Where(t => string.Equals(t.Category, categoryFilter, StringComparison.OrdinalIgnoreCase));
        }

        foreach (var item in filtered.OrderBy(t => t.Category).ThenBy(t => t.Id))
        {
            AdvancedTweaks.Add(item);
        }
    }

    private void InitializeFilters()
    {
        RiskFilters.Clear();
        RiskFilters.Add(new KeyValuePair<string, string>("Todos", string.Empty));
        RiskFilters.Add(new KeyValuePair<string, string>("Seguro", "safe"));
        RiskFilters.Add(new KeyValuePair<string, string>("Avanzado", "advanced"));
        RiskFilters.Add(new KeyValuePair<string, string>("Experimental", "experimental"));

        _selectedRiskFilter = string.Empty;
        _selectedCategoryFilter = string.Empty;
    }

    private void RefreshCategoryFilters()
    {
        CategoryFilters.Clear();
        CategoryFilters.Add(new KeyValuePair<string, string>("Todas", string.Empty));

        foreach (var category in _allTweaks.Select(t => t.Category)
                     .Where(category => !string.IsNullOrWhiteSpace(category))
                     .Distinct(StringComparer.OrdinalIgnoreCase)
                     .OrderBy(category => category))
        {
            CategoryFilters.Add(new KeyValuePair<string, string>(category, category));
        }

        SelectedCategoryFilter = string.Empty;
    }

    private static string CreateProfileContextId(string profileId)
    {
        return $"profile:{profileId}:{DateTime.UtcNow:yyyyMMddHHmmss}";
    }

    private static string ResolveLogPath()
    {
        var overridePath = Environment.GetEnvironmentVariable("LJOKERL_LOG_PATH");
        if (!string.IsNullOrWhiteSpace(overridePath))
        {
            return overridePath;
        }

        return Path.Combine(AppContext.BaseDirectory, "Logs", "execution.jsonl");
    }

    private static string ResolveConfigRoot()
    {
        var overridePath = Environment.GetEnvironmentVariable("LJOKERL_CONFIG_PATH");
        if (!string.IsNullOrWhiteSpace(overridePath))
        {
            return overridePath;
        }

        return Path.Combine(AppContext.BaseDirectory, "Config");
    }

    private static string ResolveSettingsPath()
    {
        var overridePath = Environment.GetEnvironmentVariable("LJOKERL_SETTINGS_PATH");
        if (!string.IsNullOrWhiteSpace(overridePath))
        {
            return overridePath;
        }

        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        return Path.Combine(appData, "LJokerL_TTV", "settings.json");
    }

    private static string ResolveLogExportPath(bool currentPage, int pageNumber)
    {
        var overridePath = Environment.GetEnvironmentVariable("LJOKERL_LOG_EXPORT_PATH");
        if (!string.IsNullOrWhiteSpace(overridePath))
        {
            return overridePath;
        }

        var directory = Path.Combine(AppContext.BaseDirectory, "Logs");
        var suffix = currentPage ? $"_page{pageNumber}" : string.Empty;
        var filename = $"execution_export_{DateTime.UtcNow:yyyyMMdd_HHmmss}{suffix}.jsonl";
        return Path.Combine(directory, filename);
    }

    private static string ResolveMetricsLogPath()
    {
        var overridePath = Environment.GetEnvironmentVariable("LJOKERL_METRICS_LOG_PATH");
        if (!string.IsNullOrWhiteSpace(overridePath))
        {
            return overridePath;
        }

        return Path.Combine(AppContext.BaseDirectory, "Logs", "metrics.jsonl");
    }

    private static string ResolveBenchmarkResultsPath()
    {
        var overridePath = Environment.GetEnvironmentVariable("LJOKERL_BENCHMARK_RESULTS_PATH");
        if (!string.IsNullOrWhiteSpace(overridePath))
        {
            return overridePath;
        }

        return Path.Combine(AppContext.BaseDirectory, "Benchmarks", "Results");
    }

    private void InitializeMonitoring()
    {
        var intervalSeconds = Math.Max(1, _settings.MonitoringIntervalSeconds);
        _monitorIntervalMs = intervalSeconds * 1000;
        _isMonitoringEnabled = _settings.MonitoringEnabled;
        _metricsLoggingEnabled = _settings.MetricsLoggingEnabled;
        _createRestorePointBeforeApply = _settings.CreateRestorePointBeforeApply;
        OnPropertyChanged(nameof(IsMonitoringEnabled));
        OnPropertyChanged(nameof(MetricsLoggingEnabled));
        OnPropertyChanged(nameof(CreateRestorePointBeforeApply));
        OnPropertyChanged(nameof(MonitoringToggleLabel));
        UpdateMonitoringState();
    }

    private void PersistMonitoringState()
    {
        _settings.MonitoringEnabled = _isMonitoringEnabled;
        _settings.MetricsLoggingEnabled = _metricsLoggingEnabled;
        _settings.MonitoringIntervalSeconds = Math.Max(1, _monitorIntervalMs / 1000);
        _settingsService.Save(_settings);
    }

    private void PersistRestorePointSetting()
    {
        _settings.CreateRestorePointBeforeApply = _createRestorePointBeforeApply;
        _settingsService.Save(_settings);
    }

    private void UpdateMonitoringState()
    {
        if (IsMonitoringEnabled)
        {
            StartMonitoring();
        }
        else
        {
            StopMonitoring();
        }
    }

    private void ToggleMonitoring()
    {
        IsMonitoringEnabled = !IsMonitoringEnabled;
    }

    private void StartMonitoring()
    {
        if (_monitorTimer != null)
        {
            return;
        }

        MonitoringStatus = $"En vivo • cada {_monitorIntervalMs / 1000}s";
        _monitorTimer = new Timer(_ => CaptureMetrics(), null, 0, _monitorIntervalMs);
    }

    private void StopMonitoring()
    {
        _monitorTimer?.Dispose();
        _monitorTimer = null;
        MonitoringStatus = "Monitoreo pausado";
    }

    private void CaptureMetrics()
    {
        if (!IsMonitoringEnabled)
        {
            return;
        }

        try
        {
            var snapshot = _metricsService.CaptureSnapshot();
            if (MetricsLoggingEnabled)
            {
                _metricsLogger.Log(snapshot);
            }

            _uiContext.Post(_ => ApplyMetrics(snapshot), null);
        }
        catch
        {
        }
    }

    private void ApplyMetrics(SystemMetricsSnapshot snapshot)
    {
        var cpuUsage = snapshot.Cpu.UsagePercent;
        CpuUsagePercent = cpuUsage;
        CpuUsageLabel = $"{cpuUsage:0}%";
        CpuTempLabel = snapshot.Cpu.TemperatureCelsius.HasValue
            ? $"{snapshot.Cpu.TemperatureCelsius.Value:0.0}°C"
            : "N/A";

        var gpuUsage = snapshot.Gpu?.UsagePercent;
        GpuUsagePercent = gpuUsage ?? 0;
        GpuUsageLabel = gpuUsage.HasValue ? $"{gpuUsage.Value:0}%" : "N/A";
        GpuNameLabel = string.IsNullOrWhiteSpace(snapshot.Gpu?.Name) ? "GPU" : snapshot.Gpu!.Name!;
        GpuVramLabel = snapshot.Gpu?.VramBytes.HasValue == true
            ? $"{snapshot.Gpu.VramBytes.Value / (1024 * 1024)} MB"
            : "VRAM N/A";

        var totalMem = snapshot.Memory.TotalBytes;
        var usedMem = totalMem > snapshot.Memory.AvailableBytes
            ? totalMem - snapshot.Memory.AvailableBytes
            : 0;
        MemoryUsagePercent = snapshot.Memory.UsagePercent;
        MemoryUsageLabel = $"{FormatBytes(usedMem)} / {FormatBytes(totalMem)}";

        var disk = snapshot.Disks.FirstOrDefault(d => d.Name.Equals("C:", StringComparison.OrdinalIgnoreCase))
                   ?? snapshot.Disks.FirstOrDefault();
        if (disk != null)
        {
            var diskUsed = disk.TotalBytes > disk.FreeBytes ? disk.TotalBytes - disk.FreeBytes : 0;
            DiskUsagePercent = disk.UsagePercent;
            DiskUsageLabel = $"{disk.Name} {FormatBytes(diskUsed)} / {FormatBytes(disk.TotalBytes)}";
        }
        else
        {
            DiskUsagePercent = 0;
            DiskUsageLabel = "Disk N/A";
        }

        var down = FormatNetworkRate(snapshot.Network.BytesReceivedPerSec);
        var up = FormatNetworkRate(snapshot.Network.BytesSentPerSec);
        NetworkUsageLabel = $"Descarga {down} / Subida {up}";

        MonitoringTimestamp = snapshot.TimestampUtc.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
        MonitoringStatus = IsMonitoringEnabled ? $"En vivo • actualizado {MonitoringTimestamp}" : "Monitoreo pausado";

        MonitoringProcesses.Clear();
        foreach (var process in snapshot.Processes)
        {
            MonitoringProcesses.Add(new ProcessUsageItem(process));
        }
    }

    private static string FormatBytes(ulong bytes)
    {
        var gb = bytes / (1024d * 1024d * 1024d);
        if (gb >= 1)
        {
            return $"{gb:0.0} GB";
        }

        var mb = bytes / (1024d * 1024d);
        return $"{mb:0.0} MB";
    }

    private static string FormatNetworkRate(double bytesPerSecond)
    {
        var mbps = bytesPerSecond * 8 / 1_000_000d;
        if (mbps >= 1)
        {
            return $"{mbps:0.0} Mbps";
        }

        var kbps = bytesPerSecond * 8 / 1_000d;
        return $"{kbps:0.0} Kbps";
    }

    private void RefreshBenchmarkRuns()
    {
        BenchmarkRuns.Clear();
        foreach (var run in _benchmarkStorage.LoadRuns(_benchmarkResultsPath))
        {
            BenchmarkRuns.Add(new BenchmarkRunItem(run));
        }

        if (BenchmarkRuns.Count > 0)
        {
            SelectedBenchmarkAfter ??= BenchmarkRuns[0];
        }

        if (BenchmarkRuns.Count > 1)
        {
            SelectedBenchmarkBefore ??= BenchmarkRuns[1];
        }

        _compareBenchmarksCommand?.RaiseCanExecuteChanged();
    }

    private async void RunBenchmarkAsync(BenchmarkRunOptions options)
    {
        if (IsBenchmarkRunning)
        {
            return;
        }

        IsBenchmarkRunning = true;
        BenchmarkStatus = $"Benchmark {options.TestType} ejecutando...";
        BenchmarkProgressLabel = "Iniciando";
        BenchmarkProgressPercent = 0;

        try
        {
            var progress = new Progress<BenchmarkProgress>(UpdateBenchmarkProgress);
            var result = await Task.Run(() => _benchmarkRunner.Run(options, progress));
            var path = _benchmarkStorage.SaveResult(result, _benchmarkResultsPath);
            BenchmarkStatus = $"Benchmark {options.TestType} completado: {path}";
        }
        catch (Exception ex)
        {
            BenchmarkStatus = $"Error en benchmark: {ex.Message}";
        }
        finally
        {
            IsBenchmarkRunning = false;
            RefreshBenchmarkRuns();
        }
    }

    private void UpdateBenchmarkProgress(BenchmarkProgress progress)
    {
        var stage = progress.Stage switch
        {
            "cpu" => "CPU",
            "memory" => "Memoria",
            "disk" => "Disco",
            "network" => "Red",
            "system" => "Sistema",
            "done" => "Listo",
            _ => "Iniciando"
        };

        var percent = progress.Total > 0
            ? progress.Completed * 100d / progress.Total
            : 0;
        if (progress.Stage == "done")
        {
            percent = 100;
        }

        BenchmarkProgressLabel = $"{stage} ({progress.Completed}/{progress.Total})";
        BenchmarkProgressPercent = Math.Round(percent, 1);
    }

    private void CompareBenchmarks()
    {
        if (SelectedBenchmarkBefore == null || SelectedBenchmarkAfter == null)
        {
            BenchmarkComparisonSummary = "Selecciona dos runs para comparar.";
            return;
        }

        var before = _benchmarkStorage.LoadResult(SelectedBenchmarkBefore.FilePath);
        var after = _benchmarkStorage.LoadResult(SelectedBenchmarkAfter.FilePath);
        if (before == null || after == null)
        {
            BenchmarkComparisonSummary = "No se pudieron cargar los resultados seleccionados.";
            return;
        }

        var comparison = BenchmarkComparer.Compare(before, after);
        BenchmarkComparisonSummary = FormatBenchmarkComparison(comparison);
    }

    private void ExportComparisonReport()
    {
        if (SelectedBenchmarkBefore == null || SelectedBenchmarkAfter == null)
        {
            BenchmarkStatus = "Selecciona dos runs para comparar.";
            return;
        }

        try
        {
            var before = _benchmarkStorage.LoadResult(SelectedBenchmarkBefore.FilePath);
            var after = _benchmarkStorage.LoadResult(SelectedBenchmarkAfter.FilePath);
            if (before == null || after == null)
            {
                BenchmarkStatus = "Error al cargar los runs seleccionados.";
                return;
            }

            var reportDir = Path.Combine(AppContext.BaseDirectory, "Benchmarks", "Reports");
            var (htmlPath, _) = BenchmarkReportGenerator.GenerateReport(before, after, reportDir);

            BenchmarkStatus = $"Reportes generados en la carpeta Benchmarks/Reports";

            var psi = new ProcessStartInfo(htmlPath)
            {
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        catch (Exception ex)
        {
            BenchmarkStatus = $"Error al exportar reporte: {ex.Message}";
        }
    }

    private string FormatBenchmarkComparison(BenchmarkComparisonResult comparison)
    {
        var lines = new List<string>();
        if (comparison.OverallBefore.HasValue && comparison.OverallAfter.HasValue)
        {
            lines.Add($"General: {comparison.OverallBefore:0} -> {comparison.OverallAfter:0} ({FormatDelta(comparison.OverallDelta, comparison.OverallPercentChange)})");
        }

        foreach (var delta in comparison.Deltas)
        {
            lines.Add($"{delta.Name}: {delta.BeforeScore:0} -> {delta.AfterScore:0} ({FormatDelta(delta.Delta, delta.PercentChange)})");
        }

        return lines.Count == 0 ? "Sin datos para comparar." : string.Join(Environment.NewLine, lines);
    }

    private static string FormatDelta(double? delta, double? percent)
    {
        if (!delta.HasValue)
        {
            return "n/a";
        }

        var sign = delta.Value >= 0 ? "+" : "";
        var deltaLabel = $"{sign}{delta.Value:0.##}";
        var percentLabel = percent.HasValue ? $"{sign}{percent.Value:0.##}%" : "n/a";
        return $"{deltaLabel}, {percentLabel}";
    }

    private void OpenBenchmarkFolder()
    {
        try
        {
            if (!Directory.Exists(_benchmarkResultsPath))
            {
                Directory.CreateDirectory(_benchmarkResultsPath);
            }

            var psi = new ProcessStartInfo("explorer.exe", $"\"{_benchmarkResultsPath}\"")
            {
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        catch (Exception ex)
        {
            BenchmarkStatus = $"Error al abrir carpeta: {ex.Message}";
        }
    }

    private bool CanCompareBenchmarks()
    {
        return !IsBenchmarkRunning && SelectedBenchmarkBefore != null && SelectedBenchmarkAfter != null;
    }

    private void InitializeVrrOptions()
    {
        VrrOverrideOptions.Clear();
        VrrOverrideOptions.Add(new KeyValuePair<string, string>("Auto", string.Empty));
        VrrOverrideOptions.Add(new KeyValuePair<string, string>("Force On", "true"));
        VrrOverrideOptions.Add(new KeyValuePair<string, string>("Force Off", "false"));

        var overrideValue = _settings.VrrOverride;
        _selectedVrrOverride = overrideValue == null ? string.Empty : (overrideValue.Value ? "true" : "false");
        OnPropertyChanged(nameof(SelectedVrrOverride));
    }

    private void ApplyVrrOverrideSelection()
    {
        if (_suppressOverrideUpdates)
        {
            return;
        }

        var value = _selectedVrrOverride.Trim().ToLowerInvariant();
        bool? vrrOverride = value switch
        {
            "true" => true,
            "false" => false,
            _ => null
        };

        if (_settings.VrrOverride == vrrOverride)
        {
            return;
        }

        _settings.VrrOverride = vrrOverride;
        _settingsService.Save(_settings);
        if (_bundle != null)
        {
            LoadHardwareAsync(_bundle.HardwareRules, _settings.VrrOverride);
        }
    }

    private void InitializeGpuOverrides()
    {
        _gpuNameOverride = _settings.GpuNameOverride ?? string.Empty;
        _gpuVendorOverride = _settings.GpuVendorOverride ?? string.Empty;
        OnPropertyChanged(nameof(GpuNameOverride));
        OnPropertyChanged(nameof(GpuVendorOverride));
    }

    private void InitializeCpuRamOverrides()
    {
        _cpuNameOverride = _settings.CpuNameOverride ?? string.Empty;
        _cpuCoresOverride = _settings.CpuCoresOverride?.ToString() ?? string.Empty;
        _cpuThreadsOverride = _settings.CpuThreadsOverride?.ToString() ?? string.Empty;
        _ramTotalGbOverride = _settings.RamTotalGbOverride?.ToString() ?? string.Empty;
        _ramSpeedOverride = _settings.RamSpeedMhzOverride?.ToString() ?? string.Empty;

        OnPropertyChanged(nameof(CpuNameOverride));
        OnPropertyChanged(nameof(CpuCoresOverride));
        OnPropertyChanged(nameof(CpuThreadsOverride));
        OnPropertyChanged(nameof(RamTotalGbOverride));
        OnPropertyChanged(nameof(RamSpeedOverride));
    }

    private void InitializeOsOverrides()
    {
        _osCaptionOverride = _settings.OsCaptionOverride ?? string.Empty;
        _osVersionOverride = _settings.OsVersionOverride ?? string.Empty;
        _osBuildOverride = _settings.OsBuildOverride ?? string.Empty;

        OnPropertyChanged(nameof(OsCaptionOverride));
        OnPropertyChanged(nameof(OsVersionOverride));
        OnPropertyChanged(nameof(OsBuildOverride));
    }

    private void ApplyGpuOverrideSelection()
    {
        if (_suppressOverrideUpdates)
        {
            return;
        }

        var name = string.IsNullOrWhiteSpace(_gpuNameOverride) ? null : _gpuNameOverride.Trim();
        var vendor = string.IsNullOrWhiteSpace(_gpuVendorOverride) ? null : _gpuVendorOverride.Trim();

        if (_settings.GpuNameOverride == name && _settings.GpuVendorOverride == vendor)
        {
            return;
        }

        _settings.GpuNameOverride = name;
        _settings.GpuVendorOverride = vendor;
        _settingsService.Save(_settings);
        if (_bundle != null)
        {
            LoadHardwareAsync(_bundle.HardwareRules, _settings.VrrOverride);
        }
    }

    private void ApplyCpuRamOverrideSelection()
    {
        if (_suppressOverrideUpdates)
        {
            return;
        }

        var name = string.IsNullOrWhiteSpace(_cpuNameOverride) ? null : _cpuNameOverride.Trim();
        var cores = int.TryParse(_cpuCoresOverride, out var coresValue) ? coresValue : (int?)null;
        var threads = int.TryParse(_cpuThreadsOverride, out var threadsValue) ? threadsValue : (int?)null;
        var ramGb = int.TryParse(_ramTotalGbOverride, out var ramGbValue) ? ramGbValue : (int?)null;
        var ramSpeed = int.TryParse(_ramSpeedOverride, out var ramSpeedValue) ? ramSpeedValue : (int?)null;

        if (_settings.CpuNameOverride == name &&
            _settings.CpuCoresOverride == cores &&
            _settings.CpuThreadsOverride == threads &&
            _settings.RamTotalGbOverride == ramGb &&
            _settings.RamSpeedMhzOverride == ramSpeed)
        {
            return;
        }

        _settings.CpuNameOverride = name;
        _settings.CpuCoresOverride = cores;
        _settings.CpuThreadsOverride = threads;
        _settings.RamTotalGbOverride = ramGb;
        _settings.RamSpeedMhzOverride = ramSpeed;
        _settingsService.Save(_settings);
        if (_bundle != null)
        {
            LoadHardwareAsync(_bundle.HardwareRules, _settings.VrrOverride);
        }
    }

    private void ApplyOsOverrideSelection()
    {
        if (_suppressOverrideUpdates)
        {
            return;
        }

        var caption = string.IsNullOrWhiteSpace(_osCaptionOverride) ? null : _osCaptionOverride.Trim();
        var version = string.IsNullOrWhiteSpace(_osVersionOverride) ? null : _osVersionOverride.Trim();
        var build = string.IsNullOrWhiteSpace(_osBuildOverride) ? null : _osBuildOverride.Trim();

        if (_settings.OsCaptionOverride == caption &&
            _settings.OsVersionOverride == version &&
            _settings.OsBuildOverride == build)
        {
            return;
        }

        _settings.OsCaptionOverride = caption;
        _settings.OsVersionOverride = version;
        _settings.OsBuildOverride = build;
        _settingsService.Save(_settings);
        if (_bundle != null)
        {
            LoadHardwareAsync(_bundle.HardwareRules, _settings.VrrOverride);
        }
    }

    private void ApplyGpuOverrides(HardwareSnapshot snapshot)
    {
        var name = _settings.GpuNameOverride;
        var vendor = _settings.GpuVendorOverride;
        if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(vendor))
        {
            return;
        }

        snapshot.Gpu ??= new GpuInfo();
        if (!string.IsNullOrWhiteSpace(name))
        {
            snapshot.Gpu.Name = name;
        }

        if (!string.IsNullOrWhiteSpace(vendor))
        {
            snapshot.Gpu.Vendor = vendor;
        }
    }

    private void ApplyCpuOverrides(HardwareSnapshot snapshot)
    {
        var name = _settings.CpuNameOverride;
        var cores = _settings.CpuCoresOverride;
        var threads = _settings.CpuThreadsOverride;
        if (string.IsNullOrWhiteSpace(name) && !cores.HasValue && !threads.HasValue)
        {
            return;
        }

        snapshot.Cpu ??= new CpuInfo();
        if (!string.IsNullOrWhiteSpace(name))
        {
            snapshot.Cpu.Name = name;
        }

        if (cores.HasValue)
        {
            snapshot.Cpu.Cores = cores.Value;
        }

        if (threads.HasValue)
        {
            snapshot.Cpu.LogicalProcessors = threads.Value;
        }
    }

    private void ApplyMemoryOverrides(HardwareSnapshot snapshot)
    {
        var ramGb = _settings.RamTotalGbOverride;
        var speed = _settings.RamSpeedMhzOverride;
        if (!ramGb.HasValue && !speed.HasValue)
        {
            return;
        }

        snapshot.Memory ??= new MemoryInfo();
        if (ramGb.HasValue)
        {
            snapshot.Memory.TotalBytes = (ulong)ramGb.Value * 1024UL * 1024UL * 1024UL;
        }

        if (speed.HasValue)
        {
            snapshot.Memory.AverageSpeedMhz = speed.Value;
        }
    }

    private void ApplyOsOverrides(HardwareSnapshot snapshot)
    {
        var caption = _settings.OsCaptionOverride;
        var version = _settings.OsVersionOverride;
        var build = _settings.OsBuildOverride;
        if (string.IsNullOrWhiteSpace(caption) && string.IsNullOrWhiteSpace(version) && string.IsNullOrWhiteSpace(build))
        {
            return;
        }

        snapshot.Os ??= new OsInfo();
        if (!string.IsNullOrWhiteSpace(caption))
        {
            snapshot.Os.Caption = caption;
        }

        if (!string.IsNullOrWhiteSpace(version))
        {
            snapshot.Os.Version = version;
        }

        if (!string.IsNullOrWhiteSpace(build))
        {
            snapshot.Os.BuildNumber = build;
        }
    }

    private void ResetHardwareOverrides()
    {
        _suppressOverrideUpdates = true;

        _settings.VrrOverride = null;
        _settings.GpuNameOverride = null;
        _settings.GpuVendorOverride = null;
        _settings.CpuNameOverride = null;
        _settings.CpuCoresOverride = null;
        _settings.CpuThreadsOverride = null;
        _settings.RamTotalGbOverride = null;
        _settings.RamSpeedMhzOverride = null;
        _settings.OsCaptionOverride = null;
        _settings.OsVersionOverride = null;
        _settings.OsBuildOverride = null;
        _settingsService.Save(_settings);

        _selectedVrrOverride = string.Empty;
        _gpuNameOverride = string.Empty;
        _gpuVendorOverride = string.Empty;
        _cpuNameOverride = string.Empty;
        _cpuCoresOverride = string.Empty;
        _cpuThreadsOverride = string.Empty;
        _ramTotalGbOverride = string.Empty;
        _ramSpeedOverride = string.Empty;
        _osCaptionOverride = string.Empty;
        _osVersionOverride = string.Empty;
        _osBuildOverride = string.Empty;

        OnPropertyChanged(nameof(SelectedVrrOverride));
        OnPropertyChanged(nameof(GpuNameOverride));
        OnPropertyChanged(nameof(GpuVendorOverride));
        OnPropertyChanged(nameof(CpuNameOverride));
        OnPropertyChanged(nameof(CpuCoresOverride));
        OnPropertyChanged(nameof(CpuThreadsOverride));
        OnPropertyChanged(nameof(RamTotalGbOverride));
        OnPropertyChanged(nameof(RamSpeedOverride));
        OnPropertyChanged(nameof(OsCaptionOverride));
        OnPropertyChanged(nameof(OsVersionOverride));
        OnPropertyChanged(nameof(OsBuildOverride));

        _suppressOverrideUpdates = false;

        if (_bundle != null)
        {
            LoadHardwareAsync(_bundle.HardwareRules, _settings.VrrOverride);
        }

        LastActionStatus = "Overrides reseteados";
    }

    private void InitializeLogFilters()
    {
        LogStatusFilters.Clear();
        LogStatusFilters.Add(new KeyValuePair<string, string>("All", string.Empty));
        LogStatusFilters.Add(new KeyValuePair<string, string>("Success", "success"));
        LogStatusFilters.Add(new KeyValuePair<string, string>("Failed", "failed"));

        LogTypeFilters.Clear();
        LogTypeFilters.Add(new KeyValuePair<string, string>("All", string.Empty));
        LogTypeFilters.Add(new KeyValuePair<string, string>("registry", "registry"));
        LogTypeFilters.Add(new KeyValuePair<string, string>("command", "command"));
        LogTypeFilters.Add(new KeyValuePair<string, string>("service", "service"));
        LogTypeFilters.Add(new KeyValuePair<string, string>("remove_app", "remove_app"));
        LogTypeFilters.Add(new KeyValuePair<string, string>("restore_app", "restore_app"));

        LogPageSizeOptions.Clear();
        LogPageSizeOptions.Add(25);
        LogPageSizeOptions.Add(50);
        LogPageSizeOptions.Add(100);
        LogPageSizeOptions.Add(200);

        _selectedLogPageSize = 50;
        _currentLogPage = 1;
        _totalLogPages = 1;

        _selectedLogStatusFilter = string.Empty;
        _selectedLogTypeFilter = string.Empty;
        _logSearchText = string.Empty;
        OnPropertyChanged(nameof(SelectedLogStatusFilter));
        OnPropertyChanged(nameof(SelectedLogTypeFilter));
        OnPropertyChanged(nameof(LogSearchText));
        OnPropertyChanged(nameof(SelectedLogPageSize));
        OnPropertyChanged(nameof(CurrentLogPage));
        OnPropertyChanged(nameof(TotalLogPages));
        OnPropertyChanged(nameof(LogPageSummary));
    }

    private void RefreshLogs()
    {
        if (_logger == null)
        {
            return;
        }

        _allLogItems.Clear();
        foreach (var entry in GetRecentLogEntries(200))
        {
            _allLogItems.Add(new LogEntryItem(entry));
        }

        ApplyLogFilters();
    }

    private IEnumerable<ExecutionLogEntry> GetRecentLogEntries(int maxItems)
    {
        var queue = new Queue<ExecutionLogEntry>(maxItems);
        foreach (var entry in _logger.ReadEntries())
        {
            if (queue.Count == maxItems)
            {
                queue.Dequeue();
            }

            queue.Enqueue(entry);
        }

        return queue.Reverse();
    }

    private void ApplyLogFilters()
    {
        ExecutionLogs.Clear();

        var filtered = GetFilteredLogItems().ToList();
        TotalLogPages = Math.Max(1, (int)Math.Ceiling(filtered.Count / (double)SelectedLogPageSize));
        if (CurrentLogPage > TotalLogPages)
        {
            CurrentLogPage = TotalLogPages;
        }

        var pageStart = (CurrentLogPage - 1) * SelectedLogPageSize;
        var pageItems = filtered.Skip(pageStart).Take(SelectedLogPageSize);

        foreach (var item in pageItems)
        {
            ExecutionLogs.Add(item);
        }

        LogStatusSummary = ExecutionLogs.Count == 0
            ? "No logs"
            : $"{ExecutionLogs.Count} / {filtered.Count} entries";

        _nextLogPageCommand.RaiseCanExecuteChanged();
        _prevLogPageCommand.RaiseCanExecuteChanged();
    }

    private IEnumerable<LogEntryItem> GetFilteredLogItems()
    {
        IEnumerable<LogEntryItem> filtered = _allLogItems;
        if (!string.IsNullOrWhiteSpace(_logSearchText))
        {
            var term = _logSearchText.Trim();
            filtered = filtered.Where(item =>
                item.ContextId.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                item.ActionType.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                item.Target.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                (item.Error ?? string.Empty).Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(_selectedLogStatusFilter))
        {
            filtered = _selectedLogStatusFilter.Equals("success", StringComparison.OrdinalIgnoreCase)
                ? filtered.Where(item => item.Success)
                : filtered.Where(item => !item.Success);
        }

        if (!string.IsNullOrWhiteSpace(_selectedLogTypeFilter))
        {
            filtered = filtered.Where(item => item.ActionType.Equals(_selectedLogTypeFilter, StringComparison.OrdinalIgnoreCase));
        }

        return filtered;
    }

    private void NextLogPage()
    {
        if (CurrentLogPage < TotalLogPages)
        {
            CurrentLogPage++;
            ApplyLogFilters();
        }
    }

    private void PrevLogPage()
    {
        if (CurrentLogPage > 1)
        {
            CurrentLogPage--;
            ApplyLogFilters();
        }
    }

    private void ExportLogs()
    {
        var exportPath = ResolveLogExportPath(false, CurrentLogPage);
        try
        {
            var directory = Path.GetDirectoryName(exportPath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var items = GetFilteredLogItems().ToList();
            if (items.Count == 0)
            {
                LastActionStatus = "No hay logs para exportar";
                return;
            }

            using var writer = new StreamWriter(exportPath, false);
            foreach (var item in items)
            {
                var json = JsonSerializer.Serialize(item.Raw);
                writer.WriteLine(json);
            }

            LastActionStatus = $"Logs exportados: {exportPath} ({items.Count})";
        }
        catch (Exception ex)
        {
            LastActionStatus = $"Error al exportar logs: {ex.Message}";
        }
    }

    private void ExportCurrentPage()
    {
        var exportPath = ResolveLogExportPath(true, CurrentLogPage);
        try
        {
            var directory = Path.GetDirectoryName(exportPath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var items = ExecutionLogs.ToList();
            if (items.Count == 0)
            {
                LastActionStatus = "No hay logs en la pagina actual";
                return;
            }

            using var writer = new StreamWriter(exportPath, false);
            foreach (var item in items)
            {
                var json = JsonSerializer.Serialize(item.Raw);
                writer.WriteLine(json);
            }

            LastActionStatus = $"Pagina exportada: {exportPath} ({items.Count})";
        }
        catch (Exception ex)
        {
            LastActionStatus = $"Error al exportar pagina: {ex.Message}";
        }
    }

    private void OpenLogFolder()
    {
        try
        {
            var folder = Path.GetDirectoryName(ResolveLogExportPath(false, CurrentLogPage))
                ?? Path.GetDirectoryName(LogPath)
                ?? AppContext.BaseDirectory;

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var psi = new ProcessStartInfo("explorer.exe", $"\"{folder}\"")
            {
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        catch (Exception ex)
        {
            LastActionStatus = $"Error al abrir carpeta: {ex.Message}";
        }
    }

    private static bool IsAdministrator()
    {
        using var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }
}

public sealed class ProfileItem
{
    public ProfileItem(ProfileDefinition profile, int tweaks, int services, int apps)
    {
        Id = profile.Id;
        Name = profile.Name;
        Description = profile.Description;
        TweakIds = profile.Tweaks;
        ServiceIds = profile.Services;
        AppIds = profile.Apps;
        Tweaks = tweaks;
        Services = services;
        Apps = apps;
    }

    public string Id { get; }
    public string Name { get; }
    public string Description { get; }
    public IReadOnlyList<string> TweakIds { get; }
    public IReadOnlyList<string> ServiceIds { get; }
    public IReadOnlyList<string> AppIds { get; }
    public int Tweaks { get; }
    public int Services { get; }
    public int Apps { get; }
}

public sealed class TweakItem
{
    public TweakItem(TweakDefinition definition)
    {
        Definition = definition;
        Id = definition.Id;
        Category = definition.Category;
        Risk = definition.Risk;
        RequiresReboot = definition.RequiresReboot;
        Description = definition.Description;
    }

    public TweakDefinition Definition { get; }
    public string Id { get; }
    public string Category { get; }
    public string Risk { get; }
    public bool RequiresReboot { get; }
    public string Description { get; }
}

public sealed class RecommendationItem
{
    public RecommendationItem(string ruleId, string detail)
    {
        RuleId = ruleId;
        Detail = detail;
    }

    public string RuleId { get; }
    public string Detail { get; }
}

public sealed class HardwareFlagItem
{
    public HardwareFlagItem(string label, string value, string source)
    {
        Label = label;
        Value = value;
        Source = source;
    }

    public string Label { get; }
    public string Value { get; }
    public string Source { get; }
}

public sealed class LogEntryItem
{
    public LogEntryItem(ExecutionLogEntry entry)
    {
        Raw = entry;
        TimestampUtc = entry.TimestampUtc;
        TimestampLocal = entry.TimestampUtc.ToLocalTime();
        TimestampLabel = TimestampLocal.ToString("yyyy-MM-dd HH:mm:ss");
        ContextId = entry.ContextId;
        ActionType = entry.ActionType;
        Mode = entry.Mode;
        Target = entry.Target;
        Success = entry.Success;
        Error = entry.Error;
        ErrorDetails = entry.ErrorDetails;
        StatusLabel = entry.Success ? "OK" : "FAIL";
    }

    public ExecutionLogEntry Raw { get; }
    public DateTime TimestampUtc { get; }
    public DateTime TimestampLocal { get; }
    public string TimestampLabel { get; }
    public string ContextId { get; }
    public string ActionType { get; }
    public string Mode { get; }
    public string Target { get; }
    public bool Success { get; }
    public string? Error { get; }
    public string? ErrorDetails { get; }
    public string StatusLabel { get; }
}

public sealed class ProcessUsageItem
{
    public ProcessUsageItem(ProcessMetrics metrics)
    {
        Name = metrics.Name;
        Pid = metrics.Id;
        MemoryMb = metrics.MemoryMb;
        CpuMinutes = metrics.CpuMinutes;
        MemoryLabel = $"{MemoryMb:0.0} MB";
        CpuLabel = $"{CpuMinutes:0.0}m";
    }

    public string Name { get; }
    public int Pid { get; }
    public double MemoryMb { get; }
    public double CpuMinutes { get; }
    public string MemoryLabel { get; }
    public string CpuLabel { get; }
}

public sealed class BenchmarkRunItem
{
    public BenchmarkRunItem(BenchmarkRunInfo info)
    {
        Info = info;
        FilePath = info.FilePath;
        FileName = info.FileName;
        TestType = info.TestType;
        StartTime = info.StartTime;
        DurationSeconds = info.DurationSeconds;
        OverallScore = info.OverallScore;
        var scoreLabel = info.OverallScore.HasValue ? info.OverallScore.Value.ToString("0") : "n/a";
        DisplayName = $"{StartTime} • {TestType} • {scoreLabel}";
        Detail = $"{FileName} • {DurationSeconds:0.0}s";
    }

    public BenchmarkRunInfo Info { get; }
    public string FilePath { get; }
    public string FileName { get; }
    public string TestType { get; }
    public string StartTime { get; }
    public double DurationSeconds { get; }
    public double? OverallScore { get; }
    public string DisplayName { get; }
    public string Detail { get; }
}
