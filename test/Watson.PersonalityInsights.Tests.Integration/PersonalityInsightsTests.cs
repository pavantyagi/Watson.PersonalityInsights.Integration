using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Watson.PersonalityInsights.Enums;
using Watson.PersonalityInsights.Models;
using Xunit;

// ReSharper disable ExceptionNotDocumented

namespace Watson.PersonalityInsights.Tests.Integration
{
    public class PersonalityInsightsTests
    {
        public PersonalityInsightsSettings Settings => GetSettings();

        private PersonalityInsightsSettings GetSettings()
        {
            var content = File.ReadAllText("config.json");
            var settings = JsonConvert.DeserializeObject<PersonalityInsightsSettings>(content);
            return settings;
        }

        [Fact]
        public async Task GetProfileAsCsvAsync_IsNotNull()
        {
            var content = File.ReadAllText("SampleContent1.txt");

            var service = new PersonalityInsightsService(Settings.Username, Settings.Password);
            var options = new ProfileOptions(content)
            {
                IncludeRaw = true,
                IncludeCsvHeaders = true,
                AcceptLanguage = AcceptLanguage.Es
            };
            var profile = await service.GetProfileAsCsvAsync(options).ConfigureAwait(false);

            Assert.False(string.IsNullOrWhiteSpace(profile));
        }

        [Fact]
        public async Task GetProfileAsync_IsNotNull()
        {
            var content = File.ReadAllText("SampleContent1.txt");

            var service = new PersonalityInsightsService(Settings.Username, Settings.Password);
            var options = new ProfileOptions(content) {AcceptLanguage = AcceptLanguage.Es};
            var profile = await service.GetProfileAsync(options).ConfigureAwait(false);

            Assert.NotNull(profile);
        }

        [Fact]
        public async Task GetProfileAsync_WithContentItems_IsNotNull()
        {
            var content1 = File.ReadAllText("SampleContent1.txt");
            var content2 = File.ReadAllText("SampleContent2.txt");
            var service = new PersonalityInsightsService(Settings.Username, Settings.Password);

            var contentItems = new List<ContentItem>
            {
                new ContentItem
                {
                    Content = content1,
                    Charset = "utf-8",
                    ContentType = "text/plain",
                    Created = DateTimeOffset.UtcNow,
                    Forward = false,
                    Id = Guid.NewGuid().ToString(),
                    ContentLanguage = ContentLanguage.En,
                    Reply = false,
                    SourceId = "blog",
                    Updated = DateTimeOffset.UtcNow,
                    UserId = "RogerEbert"
                },
                new ContentItem
                {
                    Content = content2,
                    Charset = "utf-8",
                    ContentType = "text/plain",
                    Created = DateTimeOffset.UtcNow,
                    Forward = false,
                    Id = Guid.NewGuid().ToString(),
                    ContentLanguage = ContentLanguage.En,
                    Reply = false,
                    SourceId = "blog",
                    Updated = DateTimeOffset.UtcNow,
                    UserId = "RogerEbert"
                }
            };

            var content = new Content(contentItems);
            var options = new ProfileOptions(content);
            var profile = await service.GetProfileAsync(options).ConfigureAwait(false);

            Assert.NotNull(profile);
        }
    }
}