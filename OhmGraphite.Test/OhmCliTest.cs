using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace OhmGraphite.Test
{
    public class OhmCliTest
    {
        [Fact]
        public async Task CanExecuteCliVersion()
        {
            await OhmCli.Execute(new[] { "--version" });
        }

        [Fact]
        public async Task CanExecuteCliStatusWithOldFlags()
        {
            await OhmCli.Execute(new[] { "status", "-servicename" });
        }

        [Fact]
        public async Task InitWritesConfigFile()
        {
            var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.config");
            try
            {
                await OhmCli.Execute(new[] { "init", "--config", path });
                Assert.True(File.Exists(path));
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Fact]
        public async Task InitDoesNotOverwriteWithoutForce()
        {
            var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.config");
            try
            {
                await File.WriteAllTextAsync(path, "sentinel");
                await OhmCli.Execute(new[] { "init", "--config", path });
                Assert.Equal("sentinel", await File.ReadAllTextAsync(path));

                await OhmCli.Execute(new[] { "init", "--config", path, "--force" });
                Assert.NotEqual("sentinel", await File.ReadAllTextAsync(path));
            }
            finally
            {
                File.Delete(path);
            }
        }
    }
}
