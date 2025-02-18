using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using OrchardCore.Https.Drivers;
using OrchardCore.Navigation;

namespace OrchardCore.Https;

public sealed class AdminMenu : AdminNavigationProvider
{
    private static readonly RouteValueDictionary _routeValues = new()
    {
        { "area", "OrchardCore.Settings" },
        { "groupId", HttpsSettingsDisplayDriver.GroupId },
    };

    internal readonly IStringLocalizer S;

    public AdminMenu(IStringLocalizer<AdminMenu> stringLocalizer)
    {
        S = stringLocalizer;
    }

    protected override ValueTask BuildAsync(NavigationBuilder builder)
    {
        if (NavigationHelper.UseLegacyFormat())
        {
            builder
                .Add(S["Security"], security => security
                    .Add(S["Settings"], S["Settings"].PrefixPosition(), settings => settings
                        .Add(S["HTTPS"], S["HTTPS"].PrefixPosition(), https => https
                            .Action("Index", "Admin", _routeValues)
                            .Permission(Permissions.ManageHttps)
                            .LocalNav()
                        )
                    )
                );

            return ValueTask.CompletedTask;
        }

        builder
            .Add(S["Settings"], settings => settings
                .Add(S["Security"], S["Security"].PrefixPosition(), security => security
                    .Add(S["HTTPS"], S["HTTPS"].PrefixPosition(), https => https
                        .Action("Index", "Admin", _routeValues)
                        .Permission(Permissions.ManageHttps)
                        .LocalNav()
                    )
                )
            );

        return ValueTask.CompletedTask;
    }
}
