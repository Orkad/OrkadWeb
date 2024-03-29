﻿using OrkadWeb.Application.Config.Queries;

namespace OrkadWeb.Specs.Steps;

[Binding]
public class ConfigSteps
{
    private readonly ISender sender;

    public ConfigSteps(ISender sender)
    {
        this.sender = sender;
    }

    [When(@"je récupère la configuration")]
    public async Task WhenJeRecupereLaConfiguration()
    {
        await sender.Send(new GetGlobalConfigurationQuery());
    }
}
