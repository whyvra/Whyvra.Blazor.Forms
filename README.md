# Whyvra.Blazor.Forms

[![NuGet Package](https://img.shields.io/nuget/v/Whyvra.Blazor.Forms.svg?color=blue&style=flat-square)](https://www.nuget.org/packages/Whyvra.Blazor.Forms/)
[![NuGet Package Download Count](https://img.shields.io/nuget/dt/Whyvra.Blazor.Forms?color=blue&style=flat-square)](https://www.nuget.org/packages/Whyvra.Blazor.Forms/)
[![LICENSE](https://img.shields.io/badge/license-MIT-blue?style=flat-square)](./LICENSE)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](http://makeapullrequest.com)

A dynamic form builder that binds to your model classes and creates the corresponding HTML form for you.

**IMPORTANT** : Please note that version 1.0.0 introduces breaking changes. `Whyvra.Blazor.Forms` is now a base package used to implement dynamic form builders. It exposes specific interfaces and base classes for builders and components to handle things like data binding and validation. For the Bulma implementation, please see [Whyvra.Blazor.Forms.Bulma](https://github.com/whyvra/Whyvra.Blazor.Forms.Bulma).

## Introduction

This package allows for the creation of dynamic forms with a class implementing `IFormBuilder<TModel>`. It relies on a  `IFormModel<TModel>` that is created by calling the `Build()` method on the form builder. The form model can then be passed to a `WhyvraForm` or a component implementing `WhyvraFormBase` which will output the HTML markup for your form.

This package can be used to create implementations for different frameworks. However, given the current state of Blazor and how most frameworks depend on JavaScript, only an implementation for the Bulma framework has been created.

Bulma is a CSS-only framework that does not require any external JavaScript. For this reason, it integrates well with Blazor. For the Bulma implementation, please see [Whyvra.Blazor.Forms.Bulma](https://github.com/whyvra/Whyvra.Blazor.Forms.Bulma).

## Installation

You should really only install this package if you're creating an implementation of `Whyvra.Blazor.Forms` for a specific framework.

Install the package by running the following command:

```bash
dotnet add package Whyvra.Blazor.Forms
```

## Usage

In order to create a framework-specific implementation of `Whyvra.Blazor.Forms`, you must create a class that implements `IFormBuilder<TModel>` and create components for the corresponding methods like `Input`, `FileInput`, `Number`, etc.

### Form builder

This package provides an abstract class `FormBuilder<TModel>` that implements some methods of the `IFormBuilder<TModel>` interface. The base class is meant to handle component registration and some basic methods. You must wire methods like `Input` to add your own component using the `Component` method.

It is strongly suggested that your components inherit from `WhyvraComponentBase` which exposes convenient methods for data binding and handle validation if required.

Let's have a look at the code below:

```csharp
using Whyvra.Blazor.Forms.Builders;
using Whyvra.Blazor.Forms.Components;

namespace Whyvra.Blazor.Forms.MyFramework
{
    public class MyFrameworkFormBuilder<TModel> : FormBuilder<TModel>
    {
        public override IFormBuilder<TModel> Input<TProperty>(Expression<Func<TModel, TProperty>> lambda)
        {
            // Add component
            Component<MyFrameworkInput<TModel>, TProperty>(lambda);

            // Get the field's display name and set as placeholder
            var name = CurrentComponent.Parameters["DisplayName"] as string;
            CurrentComponent.Parameters["Placeholder"] = name;

            return this;
        }
    }
}
```
This example provides an implementation for the `Input` method. 

The `Component` method is used to register the `MyFrameworkInput<TModel>` component.

Values inherited from `WhyvraComponentBase` are set in the `Component` method. The `DisplayName` parameter is retrieved and set on the `Placeholder` parameter, which is specific to this component in this example.

### Custom component

You can create your own custom components and add them using the `Component` method.

`WhyvraComponentBase` provides a set of standard properties like `DisplayName`, `InternalName`, `IsVisible`, etc. It also provides methods for data binding and validation.

Here's an example below:

```razor
@typeparam TModel
@inherits WhyvraComponentBase<TModel>

<div class="field">
    <label for="@InternalName">@DisplayName</label>
    <p class="control">
        <input type="text" id="@InternalName" class="input @(FormState == FormState.Read ? "is-static" : "")"
            placeholder="@Placeholder" value="@(GetData(FormModel.DataModel))" 
            readonly="@(FormState == FormState.Read)"
            @onchange="HandleChange"
            @onfocusout="HandleValidation" />
        @if (ValidationMessages.Any())
        {
            <p class="help is-danger">@ValidationMessages.First()</p>
        }
    </p>
</div>

@code
{
    private void HandleChange(ChangeEventArgs e)
    {
        var value = e.Value.ToString();
        SetData(FormModel.DataModel, value);
    }

    private void HandleValidation()
    {
        if (GetValidationResult == null) return;

        var result = GetValidationResult();
        ValidationMessages = result.ValidationMessages.ContainsKey(ValidationPath)
            ? result.ValidationMessages[ValidationPath]
            : Enumerable.Empty<string>();
    }
}
```

This custom component uses `GetData`, `SetData` and `FormModel` to retrieve and set data on the model class. It also uses the  `GetValidationResult` and `ValidationMessages` properties to retrieve validation messages for the current state of the model class.

## License

Released under the [MIT License](./LICENSE).