# mine-shop-idp

# add db context update
## dotnet ef database update --context ConfigurationDbContext
## dotnet ef database update --context PersistantDbContext
## dotnet ef database update --context ApplicationDbContext

# Connection string for docker 
## "DefaultConnection": "Server=localhost,1433;Database=dev-minecommerce-identity;User ID=SA;Password=P@ssw0rd;"

Steps to use Identityserver 4 with EFcore
1. Create new site with identityserver
2. add EFCore
3. add AddConfigurationStore and AddOperationalStore by using extension method
4. Add Client, IdentityResource, API scopes

**Notes: 
- Maybe samesite confige error on Chrome, please disable it so cookie can write