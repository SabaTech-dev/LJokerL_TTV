---
description: Patrones y mejores prácticas para desarrollo WPF con MVVM
version: 1.0.0
tags: [wpf, mvvm, xaml, ui, dotnet, csharp]
priority: high
---

# WPF MVVM Patterns

Patrones y mejores prácticas para desarrollo de interfaces WPF siguiendo el patrón MVVM (Model-View-ViewModel).

## Casos de uso

- Desarrollar ventanas y controles WPF
- Implementar arquitectura MVVM
- Gestionar bindings y comandos
- Crear ViewModels reactivos
- Diseñar XAML mantenible

## Estructura MVVM

```
LJokerL.App/
├── Views/
│   ├── MainWindow.xaml
│   ├── MainWindow.xaml.cs
│   └── Controls/
├── ViewModels/
│   ├── MainViewModel.cs
│   └── Base/
│       └── ViewModelBase.cs
├── Models/
│   └── ...
└── Converters/
    └── ...
```

## ViewModel Base

```csharp
public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
```

## RelayCommand

```csharp
public class RelayCommand : ICommand
{
    private readonly Action<object> _execute;
    private readonly Predicate<object> _canExecute;

    public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

    public void Execute(object parameter) => _execute(parameter);

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
}
```

## Best practices XAML

### 1. Separación de responsabilidades
- **View**: Solo presentación, no lógica
- **ViewModel**: Lógica de presentación, no conocimiento de View
- **Model**: Datos y reglas de negocio

### 2. Bindings
```xml
<!-- Bindear a propiedad del ViewModel -->
<TextBox Text="{Binding UserName, UpdateSourceTrigger=PropertyChanged}" />

<!-- Bindear comando -->
<Button Content="Guardar" Command="{Binding SaveCommand}" />

<!-- Binding con conversor -->
<TextBlock Text="{Binding Status, Converter={StaticResource StatusConverter}}" />
```

### 3. DataContext
```xml
<Window.DataContext>
    <vm:MainViewModel />
</Window.DataContext>
```

### 4. Recursos y estilos
```xml
<Window.Resources>
    <converters:StatusConverter x:Key="StatusConverter" />
    <Style TargetType="Button" BasedOn="{StaticResource {x:Type Button}}">
        <Setter Property="Padding" Value="10,5" />
    </Style>
</Window.Resources>
```

## Implementación ViewModel

```csharp
public class MainViewModel : ViewModelBase
{
    private string _title;
    private ObservableCollection<Profile> _profiles;
    private Profile _selectedProfile;
    private ICommand _applyProfileCommand;

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public ObservableCollection<Profile> Profiles
    {
        get => _profiles;
        set => SetProperty(ref _profiles, value);
    }

    public Profile SelectedProfile
    {
        get => _selectedProfile;
        set
        {
            if (SetProperty(ref _selectedProfile, value))
            {
                // Actualizar comandos cuando cambia selección
                ((RelayCommand)ApplyProfileCommand).RaiseCanExecuteChanged();
            }
        }
    }

    public ICommand ApplyProfileCommand =>
        _applyProfileCommand ??= new RelayCommand(
            param => ApplyProfile(),
            param => SelectedProfile != null);

    private async void ApplyProfile()
    {
        // Lógica para aplicar perfil
        await ExecutionEngine.ApplyProfileAsync(SelectedProfile);
    }
}
```

## Async/Await en ViewModels

```csharp
public async Task LoadProfilesAsync()
{
    IsLoading = true;
    try
    {
        var profiles = await ConfigLoader.LoadProfilesAsync();
        Profiles = new ObservableCollection<Profile>(profiles);
    }
    catch (Exception ex)
    {
        // Manejar error
        await ShowErrorAsync(ex.Message);
    }
    finally
    {
        IsLoading = false;
    }
}
```

## Notificación de cambios en colecciones

```csharp
// Usar ObservableCollection para notificaciones automáticas
public ObservableCollection<OptimizationResult> Results { get; } = 
    new ObservableCollection<OptimizationResult>();

// Para cambios en propiedades de items
public class OptimizationResult : INotifyPropertyChanged
{
    // Implementar INotifyPropertyChanged
}
```

## Validación

```csharp
public class Profile : INotifyPropertyChanged, IDataErrorInfo
{
    public string this[string columnName]
    {
        get
        {
            switch (columnName)
            {
                case nameof(Name):
                    return string.IsNullOrWhiteSpace(Name) ? "Nombre requerido" : null;
                default:
                    return null;
            }
        }
    }

    public string Error => null;
}
```

## Dependencias

- dotnet-csharp: Convenciones de código C#
- windows-optimization: Para integración con UI de optimizaciones

## Notas

- Mantener ViewModels independientes de la View
- Usar `async/await` para operaciones I/O
- Implementar `INotifyPropertyChanged` para bindings
- Usar `ObservableCollection` para colecciones
- Validar datos con `IDataErrorInfo`
