# Whyvra.Blazor.Forms

[![NuGet Package](https://img.shields.io/nuget/v/Whyvra.Blazor.Forms.svg?color=blue&style=flat-square)](https://www.nuget.org/packages/Whyvra.Blazor.Forms/)
[![NuGet Package Download Count](https://img.shields.io/nuget/dt/Whyvra.Blazor.Forms?color=blue&style=flat-square)](https://www.nuget.org/packages/Whyvra.Blazor.Forms/)
[![LICENSE](https://img.shields.io/badge/license-MIT-blue?style=flat-square)](./LICENSE)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](http://makeapullrequest.com)

A dynamic form builder that binds to your model classes and creates the corresponding HTML form for you.

## Introduction

Create dynamic forms with the generic `FormBuilder<T>` class and then get a `FormModel<T>` by calling the `Build()` method on the form builder. The form model can then be passed to a `FormRenderer` which will output the HTML markup for your form.

This could be used to create renderers for different frameworks. However, given the current state of Blazor and how most frameworks depend on JavaScript, only a renderer for the Bulma framework has been created.

Bulma is a CSS-only framework that does not require any external JavaScript. For this reason, it integrates well with Blazor.

## Installation

```bash
dotnet add package Whyvra.Blazor.Forms
```

In your Blazor project, add reference to the Bulma CSS to your `wwwroot/index.html`:

```html
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bulma@0.9.1/css/bulma.min.css">

<!-- Also add the Font Awesome icons, if you intend to use them -->
<script defer src="https://use.fontawesome.com/releases/v5.14.0/js/all.js"></script>
```

You can also find some alternative themes for Bulma [here](https://jenil.github.io/bulmaswatch/).

## Usage

Given the following model class:

```csharp
public class Contact
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}
```

The generic `FormBuilder<T>` class can be used to define elements on a form. The resulting `FormModel` can then be used a `FormRenderer` to generate the corresponding HTML `<form>`.

```razor
@using System.Text.Json
@using Whyvra.Blazor.Forms
@using Whyvra.Blazor.Forms.Renderers

<BulmaForm FormModel="formModel" FormState="FormState.New">
</BulmaForm>

<button class="button is-success" @onclick="ProcessForm">Done</button>

@code
{
  private FormModel<Contact> formModel;

  protected override void OnInitialized()
  {
    formModel = new FormBuilder<Contact>()
      .Input(x => x.FirstName)
      .Input(x => x.LastName)
      .Input(x => x.Email)
      .Build();
  }

  private void ProcessForm()
  {
    var model = formModel.DataModel;
    Console.WriteLine(JsonSerializer.Serialize(model));
  }
}
```

## Form Validation

In order to validate the form, you can supply pass a `Func<string, IEnumerable<string>>` parameter called GetValidationMessages to the `FormRenderer`. The provided function should return a list of error messages for the PropertyName that gets passed.

Given the following validator for the `Contact` model class :

```csharp
public class ContactValidator : AbstractValidator<Contact>
{
    public ContactValidator()
    {
        RuleFor(x => x.FirstName).NotNull().NotEmpty().MaximumLength(32);
        RuleFor(x => x.LastName).NotNull().NotEmpty().MaximumLength(32);
        RuleFor(x => x.Email).EmailAddress();
    }
}
```

The `ValidateProperty` function that will return a list of error messages for each property matching the given `key`. This function can be passed as the `GetValidationMessages` paramter on the `BulmaForm` razor component.

```razor
@using System.Text.Json
@using FluentValidation
@using Whyvra.Blazor.Forms
@using Whyvra.Blazor.Forms.Renderers

@inject IValidator<Contact> Validator

<BulmaForm @ref="form" FormModel="formModel" FormState="FormState.New" GetValidationMessages="ValidateProperty">
</BulmaForm>

<button class="button is-success" @onclick="ProcessForm">Done</button>

@code
{
  private IFormRenderer form;
  private FormModel<Contact> formModel;

  protected override void OnInitialized()
  {
    formModel = new FormBuilder<Contact>()
      .Input(x => x.FirstName)
      .Input(x => x.LastName)
      .Input(x => x.Email)
      .Build();
  }

  private void ProcessForm()
  {
    var model = formModel.DataModel;
    var result = Validator.Validate(model);

    if (result.IsValid)
    {
      Console.WriteLine(JsonSerializer.Serialize(model));
    }
    else
    {
      // Trigger validation on whole form
      form.ValidateForm();
    }
  }

  private IEnumerable<string> ValidateProperty(string key)
  {
    var result = Validator.Validate(formModel.DataModel);

    return result.Errors
        .Where(x => x.PropertyName.Equals(key))
        .Select(x => x.ErrorMessage);
  }
}
```

## License

Released under the [MIT License](./LICENSE).