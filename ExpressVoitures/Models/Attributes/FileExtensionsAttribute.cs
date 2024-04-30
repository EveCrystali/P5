using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

public class FileExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _extensions;
    public FileExtensionsAttribute(string[] extensions)
    {
        _extensions = extensions;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var files = value as List<IFormFile>;
        if (files != null)
        {
            foreach (var file in files)
            {
                string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_extensions.Contains(extension))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }
        }

        return ValidationResult.Success;
    }

    public string GetErrorMessage()
    {
        return $"Invalid file type. Allowed types are {string.Join(", ", _extensions)}.";
    }
}
