# Pipes.Net

||Badge|
|------|:------:|
|**Target Frameworks**|[![Targets](https://img.shields.io/badge/.NET%20-7-green.svg)](https://learn.microsoft.com/en-us/dotnet/core/introduction)|
|**Downloads**|[![](https://img.shields.io/nuget/dt/Pipes.Net.svg)](https://www.nuget.org/packages/Pipes.Net/)|
|**Issues**|[![](https://img.shields.io/github/issues/playunits/Pipes.Net.svg)](https://github.com/playunits/Pipes.Net/issues)|
|**Version**|[![](https://img.shields.io/nuget/v/Pipes.Net.svg)](https://www.nuget.org/packages/Pipes.Net/)|

Pipes.Net aims to bundle reoccuring simple workflows in an interactive object structure.

It can be easily extended with new nodes and features, as well as modifying the existing features to match any challenges.


## Installation

The library is published to [NuGet](https://www.nuget.org/packages/Pipes.Net/0.1.0) and can be installed through the .NET CLI

```bat
> dotnet add package Pipes.Net
```

or the [Visual Studio Package Manager](http://docs.nuget.org/docs/start-here/using-the-package-manager-console)

```powershell
PM> Install-Package Pipes.Net
```

## Getting Started

Every Piece of Code utilizing Pipes.Net needs to include the following using clauses:

```csharp
using Pipes.Net;
using Pipes.Net.Extensions;
```

Pipelines can be created using an Factory Pattern:

```csharp
var factory = new PipelineFactory();
var pipeline = factory.Build();
```

You can add content to the pipeline using the specified Factory functions:

```csharp
var pipeline = new PipelineFactory()
    .AddAction(() => Console.WriteLine("Hello World"))
    .Build();
```

From default there are additional Items enabling a more complex possibility of building Pipelines:

```csharp
var pipeline = new PipelineFactory()
    .AddDecision<int>(x => x >= 30, success => 
        success.AddAction<int>((score) => Console.WriteLine($"Passed Test with a score of {score}"))
            .AddAction<int>((score) => 6-5 * score / 100)
            .AddSplit(MergeConditions.AllFinished,
                x => x.AddAction<int>((grade) => Console.WriteLine($"Grade: {grade}")),
                x => x.AddAction(() => Console.WriteLine("Storing Grade on Server"))
                    .AddAction(() => Thread.Sleep(3000))// Simulate some sort of Time consuming Action                    
            )
            .AddAction(() => Console.WriteLine($"Grading and uploading has finished"))
        ,failure => failure.AddAction((score) => Console.WriteLine($"Score of {score} does not qualify as a passing score"))
    ).Build();      
```

The Pipeline can then be run like this:

```csharp
await pipeline.Run(56);
```

and the Output will look like this:

```bat
Passed Test with a score of 56
Grade: 4
Storing Grade on Server
Grading and uploading has finished
```

You can also run the same Pipeline multiple times using different Inputs:

```csharp
await pipeline.Run(98);
```

that will result in different Outputs

```bat
Passed Test with a score of 98
Grade: 2
Storing Grade on Server
Grading and uploading has finished
```

Head to the [wiki](https://github.com/playunits/Pipes.Net/wiki) for more examples, or [check out the code of the example project](https://github.com/playunits/Pipes.Net/tree/master/src/Pipes.Net.Example).

## Contributing
You are welcome to share any improvement Ideas or Bugs you encoutered in the [issues page](https://github.com/playunits/Pipes.Net/issues/new).

It is greatly appreciated if you take the time to fork and contribute to this project.