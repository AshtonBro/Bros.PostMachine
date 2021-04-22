using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Bros.PostMachine
{
    class CustomSettings
    {
        private readonly object LockObject = new object();

        private static CustomSettings _Instance;
        public static CustomSettings Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new CustomSettings();
                return _Instance;
            }
        }

        private static FileInfo CurrentFile
        {
            get
            {
                var exeFile = new FileInfo(Assembly.GetExecutingAssembly().Location);
                var cfgFile = new FileInfo(exeFile.Directory.FullName + @"\settings.cfg");
                return cfgFile;
            }
        }

        public CustomSettings()
        {
            var file = CurrentFile;

            if (!file.Exists)
            {
                Save();
            }
            else
            {
                Reload();
            }
        }

        public ulong VkUserId { get; set; } = 0;
        public string VkLogin { get; set; } = "";
        public string VkPassword { get; set; } = "";
        public string VkAccessToken { get; set; } = "";
        public string VkApplicationId { get; set; } = "";

        public void Save()
        {
            lock (LockObject)
            {
                var file = CurrentFile;

                if (file.Exists)
                {
                    file.Delete();
                }

                var stream = file.Create();

                var lines = this.GetType().GetProperties(BindingFlags.Public |
                    BindingFlags.Instance).Select(f => $"{f.Name}={f.GetValue(this)}");

                var bytes = Encoding.ASCII.GetBytes(string.Join(Environment.NewLine, lines));

                stream.Write(bytes);
                stream.Close();
            }
        }

        public void Reload()
        {
            lock (LockObject)
            {
                var file = CurrentFile;
                var lines = System.IO.File.ReadLines(file.FullName);

                foreach (string f in lines)
                {
                    try
                    {
                        var result = string.Empty;
                        result = f.Split("//").FirstOrDefault();
                        result = result.Trim();

                        if ((string.IsNullOrWhiteSpace(result)))
                            continue;

                        if (!f.Contains("="))
                            Console.WriteLine($"Bad setting: {result}");

                        var prop = this.GetType().GetProperty(result.Split("=").First().Trim());

                        if (prop == null)
                            Console.WriteLine($"Invalid setting: {result}");

                        object val = result.Split("=").Last().Trim();

                        var parser = prop.PropertyType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                        .Where(ff => ff.Name == "Parse")
                        .FirstOrDefault(ff => ff.GetParameters().Count() == 1 && ff.GetParameters().First().ParameterType == typeof(string));

                        if (parser != null)
                            val = parser.Invoke(null, new[] { val });

                        prop.SetValue(this, val);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Cannot apply setting: {f} - {ex.Message}");
                    }
                }
            }
        }
    }
}
