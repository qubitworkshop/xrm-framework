﻿# XRM FRAMEWORK

Xrm-Framework is a Dynamics 365 customization framework that facilitate complete test driven development of CRM plugins. What's more; You don't even need CRM to develop plugins anymore. You can mock the entire CRM platform using the built in JSON framework.

## Features
### Core Framework
- Built around Ninject dependency injection framework to support easy testability and mocking
- Built in Serilog Logging framework with the following supported sinks: SEQ, MS SQL Server, File, Windows Event Log and Splunk
- Implements an easy to use event pipeline model, which allows you to implement multiple pipeline event handlers in one plugin
- Implements a settings provider for easy configuration and access of settings
- Provides mechanisms to handles custom exceptions and ways for you to bubble them up to the Dynamics platform

### Test Framework
- Mock using just embedded JSON files to define the CRM context
- Test framework built using FakeXrmEasy365 

## Getting Started
### Prerequisites
- Visual Studio 2017 or higher
- .Net Framework 4.6.1

### Creating Plugins
```
Best documentaion is to download the source from the GitHub repo and studying the samples present
```

Start with installing the latest version of the package from the package manager console in Visual Studio
```
Install-Package Qubit.Xrm.Framework
```
This will install quite a bit of dependencies along with the main package.

After installation you can now start creating your first plugin. Before we do that, let us look at the different types of plugins you can create:

#### Entity Pipeline Plugins

These are plugins that follow a pipeline pattern for CRM entities. What this means is that each message and stage in the plugin's pipeline is considered a pipeline event. You will need two things to execute a pipeline plugin. An derivation of EntityPipelineService abstract class and the plugin itself. Everything else is automatically taken care of.

```C#
public interface ISampleEntityPipelineService : IEntityPipelineService
{ }

public class SampleEntityPipelineService : EntityPipelineService, ISampleEntityPipelineService
{
    private readonly IOrganizationService _organizationService;
    private readonly ISettingsProvider _settingsProvider;

    public SampleEntityPipelineService(IPluginExecutionContextAccessor executionContextAccessor, ILogger logger,
        IOrganizationService organizationService, ISettingsProvider settingsProvider) :
        base(executionContextAccessor, logger)
    {
        _organizationService = organizationService;
        _settingsProvider = settingsProvider;
    }

    protected override void OnCreated()
    {
        Entity account = ExecutionContextAccessor.Target.GetEntity(new ColumnSet());
        account["qubit_autonumber"] = "12345";


        using (WebClient client = new WebClient())
        {
            string url = _settingsProvider.Get("abnservice");
            account["qubit_abn"] = client.DownloadString($"{url.TrimEnd('/')}/validate");
        }
            
        _organizationService.Update(account);
    }
}

[Messages(Messages.Create, Messages.Update)]
[TargetEntityLogicalName("account")]
public sealed class SampleEntityPipelinePlugin : EntityPipelinePlugin<ISampleEntityPipelineService>
{
    public SampleEntityPipelinePlugin()
    { }

    public SampleEntityPipelinePlugin(IKernel fakeServices, Action<IKernel> setupMockServices) 
        : base(fakeServices, setupMockServices)
    { }
}
```
The advantage of using a pipeline plugin is that you now just have to implement the pipeline services and its various overrides to achieve your customizations in a more readable code base. This is because this exposes the plugins pipeline stage where the code will be executed, rather than relying on plugin registration information.

#### Standard Plugins
Standard plugins are the tradational plugins that we all have loved to embrace in CRM.

```C#
[Messages(Messages.Create, Messages.Update)]
[TargetEntityLogicalName("account")]
public sealed class SamplePlugin : Qubit.Xrm.Framework.Plugin
{
    public SamplePlugin()
    { }

    public SamplePlugin(IKernel fakeServices, Action<IKernel> setupMockServices) 
        : base(fakeServices, setupMockServices)
    { }

    public override void Execute(IKernel services)
    {
        IOrganizationService organizationService = services.Get<IOrganizationService>();
    }
}
```
There is nothing special about this implementation except all the service are now neatly registerd into the ninject container.

### Registering Your Own Services
You can register your own services through an override of the configure method.

```C#
public interface ISomeService
{
    void DoSomethingSpectacular();
}

public class SomeService : ISomeService
{
    public void DoSomethingSpectacular()
    {
        //
    }
}

[Messages(Messages.Create, Messages.Update)]
[TargetEntityLogicalName("account")]
public sealed class SamplePlugin : Qubit.Xrm.Framework.Plugin
{
    public SamplePlugin()
    { }

    public SamplePlugin(IKernel fakeServices, Action<IKernel> setupMockServices) 
        : base(fakeServices, setupMockServices)
    { }

    public override void Execute(IKernel services)
    {
        IOrganizationService organizationService = services.Get<IOrganizationService>();
    }

    public override void Configure(IKernel services)
    {
        services.Bind<ISomeService>().To<SomeService>().InTransientScope();
    }
}
```

### API Reference
All services can be accessed through ninject's container (IKernel)
```C#
public override void Execute(IKernel services)
{
    IOrganizationService organizationService = services.Get<IOrganizationService>();
}
```
#### Built In Services
##### Plugin Execution Context Accessor (IPluginExecutionContextAccessor)
The plugin execution context accessor gives you access to the following members:
- Stage: the pipeline stage
- MessageName: the message name
- Target: information about the context entity
- User: information about the context user
- HandleException: handle any exception you want to handle in your code through the framework's prescribed exception handler

##### Settings Provider (ISettingsProvider)
The settings provider is a simple application settings store that provides configuration settings to various parts of the framework. It could be as simple as a hard coded key/value store or as complex as a SQL server based store.

A simple custom settings provider is shown below

```C#
public class CustomSettingsProvider : DefaultSettingsProvider
{
    private Dictionary<string, string> _store;

    public CustomSettingsProvider(IOrganizationService organizationService, ICache cache)
        : base(organizationService, cache)
    {
        _store = new Dictionary<string, string>
        {
            { "Logging", "{\"SourceName\": \"XrmFrameworkTests\",\"Sink\": \"Console\" }" }
        };
    }

    public override string Get(string key)
    {
        return _store[key];
    }
}
```
Another example where you can fully customize the provider

```C#
public class CustomSettingsProvider : ISettingsProvider
{
    private Dictionary<string, string> _store;

    public CustomSettingsProvider()
    {
        _store = new Dictionary<string, string>
        {
            { "Logging", "{\"SourceName\": \"XrmFrameworkTests\",\"Sink\": \"Console\" }" }
        };
    }

    public string Get(string key)
    {
        return _store[key];
    }

    public T Get<T>(string key) where T : class
    {
        string jsonString = Get(key);
        return JsonConvert.DeserializeObject<T>(jsonString);
    }
}
```

If you don't already have a configuration infrastrucutre setup, you can use the built in settings provider (DefaultSettingsProvider). To use this, you will also have to install the Qubit Settings Provider managed solution in your CRM system.

To use your custom settings provider use it as a type parameter to the Plugin abstract class
```C#
[Messages(Messages.Create, Messages.Update)]
[TargetEntityLogicalName("account")]
public sealed class SamplePlugin : Qubit.Xrm.Framework.Plugin<CustomSettingsProvider>
{
    public SamplePlugin()
    { }

    public SamplePlugin(IKernel fakeServices, Action<IKernel> setupMockServices) : base(fakeServices, setupMockServices)
    { }

    public override void Execute(IKernel services)
    {
        IOrganizationService organizationService = services.Get<IOrganizationService>();
    }

    public override void Configure(IKernel services)
    {
        services.Bind<ISomeService>().To<SomeService>().InTransientScope();
    }
}
```

Derive from Qubit.Xrm.Framework.Plugin and the default settings provider will automatically be used 
```C#
[Messages(Messages.Create, Messages.Update)]
[TargetEntityLogicalName("account")]
public sealed class SamplePlugin : Qubit.Xrm.Framework.Plugin
{
    public SamplePlugin()
    { }

    public SamplePlugin(IKernel fakeServices, Action<IKernel> setupMockServices) : base(fakeServices, setupMockServices)
    { }

    public override void Execute(IKernel services)
    {
        IOrganizationService organizationService = services.Get<IOrganizationService>();
    }

    public override void Configure(IKernel services)
    {
        services.Bind<ISomeService>().To<SomeService>().InTransientScope();
    }
}
```
##### Logging (ILogger)
##### Cache Provider (ICache)
## Testing

A step by step series of examples that tell you how to get a development env running

Say what the step will be

```
Give the example
```

And repeat

```
until finished
```

End with an example of getting some data out of the system or using it for a little demo

## Running the tests

Explain how to run the automated tests for this system

### Break down into end to end tests

Explain what these tests test and why

```
Give an example
```

## Deployment

Add additional notes about how to deploy this on a live system

## Built With

* [Ninject](https://github.com/ninject/Ninject) - Dependency Management
* [Serilog](https://serilog.net/) - Logging
* [pub-comp/caching](https://github.com/pub-comp/caching) - Caching
* [CodeGuard](https://github.com/3komma14/Guard) - Validation and Assertions
* [Newtonsoft.Json](https://www.newtonsoft.com/json) - JSON serialization/deserialization
* [FakeXrmEasy](https://github.com/jordimontana82/fake-xrm-easy) - TDD Framework
* [NUnit](http://nunit.org/) - Unit testing 
* [WireMock](https://github.com/WireMock-Net/WireMock.Net) - Mocking the Web

## Contributing

Please read [CONTRIBUTING.md](https://github.com/qubitworkshop/xrm-framework/blob/master/CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/qubitworkshop/xrm-framework/tags). 

## Authors

* **Arun Murugan** - *Initial work* - [terminalxposure](https://github.com/terminalxposure)

See also the list of [contributors](https://github.com/qubitworkshop/xrm-framework/graphs/contributors) who participated in this project.

## License

This project is licensed under the Simple Public License 2.0 - see the [LICENSE.md](https://github.com/qubitworkshop/xrm-framework/blob/master/LICENSE.md) file for details

## Acknowledgments

* Hat tip to anyone whose code was used
* Inspiration
* etc
