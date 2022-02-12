using System.Text.RegularExpressions;
using Licensing.Utilities;
using DeviceId;

namespace Licensing.Services;

public class SoftwareGenuineService
{
    private readonly GenerateUniqueValueUtility _guv = new GenerateUniqueValueUtility();
    private readonly DeviceIdBuilder _deviceId = new DeviceIdBuilder();
    private readonly Random _random = new Random();

    string[,] TYPES = new string[5, 5] {
            { "G", "N", "O", "Z", "P" },
            { "L", "M", "Y", "I", "A" },
            { "B", "F", "W", "E", "Q" },
            { "D", "H", "K", "R", "S" },
            { "C", "V", "U", "J", "X" }
        };
    const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    const int DEVICEID_LENGTH = 20;
    const int DEVICEID_PARTONE = 5;
    const int DEVICEID_PARTTWO = 15;

    public SoftwareGenuineService()
    {
    }

    public string GenerateActivation()
    {
        var type = Convert.ToString(GetRandomChar(CHARS, _random));

        var salts = _guv.Token(3, 3).Split("-");

        var deviceId = GetDeviceId();


        var deviceIdPartOne = deviceId.Substring(0, 5);
        var deviceIdPartTwo = deviceId.Substring(5, 15);

        //type1 + idOne5 + saltOne3 + saltTwo3 + saltThree3 + idTwo15 
        var activation = type
            + deviceIdPartOne
            + salts[0]
            + salts[1]
            + salts[2]
            + deviceIdPartTwo;

        activation = Regex.Replace(activation, ".{5}", "$0-");
        activation = activation.Remove(activation.Length - 1, 1);

        return activation;
    }

    public string GenerateLicense(string activation)
    {
        var typeIndex = _random.Next(4);
        var typeValue = _random.Next(4);
        var type = TYPES[typeIndex, typeValue];

        var salts = _guv.Token(3, 3).Split("-");

        activation = activation.Replace("-", "");
        var deviceIdPartOne = activation.Substring(1, DEVICEID_PARTONE);
        var deviceIdPartTwo = activation.Substring(15, DEVICEID_PARTTWO);

        string license = type;
        switch (typeIndex)
        {
            case 0:
                license += salts[0]
                        + deviceIdPartOne
                        + salts[1]
                        + deviceIdPartTwo
                        + salts[2];
                break;
            case 1:
                license += deviceIdPartOne
                        + salts[0]
                        + salts[1]
                        + deviceIdPartTwo
                        + salts[2];
                break;
            case 2:
                license += salts[0]
                        + deviceIdPartOne
                        + salts[1]
                        + salts[2]
                        + deviceIdPartTwo;
                break;
            case 3:
                license += salts[0]
                        + salts[1]
                        + deviceIdPartOne
                        + salts[2]
                        + deviceIdPartTwo;
                break;
            case 4:
                license += salts[0]
                        + deviceIdPartOne
                        + deviceIdPartTwo
                        + salts[1]
                        + salts[2];
                break;
            default:
                return "";
        }

        license = Regex.Replace(license, ".{5}", "$0-");
        license = license.Remove(license.Length - 1, 1);

        return license;
    }

    public bool CheckLicense(string license)
    {
        var deviceId = GetDeviceId();

        license = license.Replace("-", "");

        var typeKey = Convert.ToString(license[0]);
        var type = -1;
        var index = 0;
        foreach (var typeList in TYPES)
        {
            if (typeList == typeKey)
            {
                type = index / 5;
                break;
            }
            index++;
        }

        string licenseDeviceId;
        switch (type)
        {
            case 0:
                licenseDeviceId = license.Substring(4, DEVICEID_PARTONE)
                                + license.Substring(12, DEVICEID_PARTTWO);
                break;
            case 1:
                licenseDeviceId = license.Substring(1, DEVICEID_PARTONE)
                                + license.Substring(12, DEVICEID_PARTTWO);
                break;
            case 2:
                licenseDeviceId = license.Substring(4, DEVICEID_PARTONE)
                                + license.Substring(15, DEVICEID_PARTTWO);
                break;
            case 3:
                licenseDeviceId = license.Substring(7, DEVICEID_PARTONE)
                                + license.Substring(15, DEVICEID_PARTTWO);
                break;
            case 4:
                licenseDeviceId = license.Substring(4, DEVICEID_PARTONE)
                                + license.Substring(9, DEVICEID_PARTTWO);
                break;
            default:
                return false;
        }

        return deviceId == licenseDeviceId;
    }

    private char GetRandomChar(string text, Random random)
    {
        int index = random.Next(text.Length);
        return text[index];
    }

    private string GetDeviceId()
    {

        var deviceId = _deviceId
            .AddMacAddress()
            .AddMachineName()
            .ToString();
        Regex rgx = new Regex("[^a-zA-Z0-9]");
        deviceId = rgx.Replace(deviceId, "");

        deviceId = deviceId
            .Substring(0, DEVICEID_LENGTH)
            .ToUpper();
        return deviceId;
    }

    private string EncryptDeviceId(string deviceId)
    {
        char[] charArr = deviceId.ToCharArray();
        return "";
    }

    private string DecryptDeviceId(string encryptedDeviceId)
    {
        return "";
    }
}
