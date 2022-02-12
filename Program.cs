// See https://aka.ms/new-console-template for more information
using Licensing.Services;

Console.WriteLine("Hello, World!");

var _genuine = new SoftwareGenuineService();

var activation = _genuine.GenerateActivation();
Console.WriteLine("ACTIVATION CODE: " + activation);

var license = _genuine.GenerateLicense(activation);
Console.WriteLine("LICENSE CODE: " + license);

var isValid = _genuine.CheckLicense(license);
Console.WriteLine(isValid);




Console.ReadLine();
