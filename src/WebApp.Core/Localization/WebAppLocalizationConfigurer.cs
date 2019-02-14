using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Json;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace WebApp.Localization
{
    public static class WebAppLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Languages.Add(new LanguageInfo("zh-CN", "简体中文", "中国", isDefault: true));
            //localizationConfiguration.Languages.Add(new LanguageInfo("tr", "Türkçe", "famfamfam-flags tr"));

            //localizationConfiguration.Sources.Add(
            //    new DictionaryBasedLocalizationSource(WebAppConsts.LocalizationSourceName,
            //        new JsonEmbeddedFileLocalizationDictionaryProvider(
            //            typeof(WebAppLocalizationConfigurer).GetAssembly(),
            //            "WebApp.Localization.SourceFiles"
            //        )
            //    )
            //);

            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(WebAppConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(WebAppLocalizationConfigurer).GetAssembly(),
                        "WebApp.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}