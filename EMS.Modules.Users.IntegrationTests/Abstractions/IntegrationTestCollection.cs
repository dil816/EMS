namespace EMS.Modules.Users.IntegrationTests.Abstractions;

[CollectionDefinition(nameof(IntegrationTestCollection))]
public class IntegrationTestCollection : ICollectionFixture<IntegrationTestWebAppFactory>;

