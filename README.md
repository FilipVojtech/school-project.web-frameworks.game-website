# Development

The project doesn't need much to run

1. Obtain Keys and Secrets for Google, Facebook and Twitter through their respective developer portals
2. Install Docker
3. Setup and run docker image using
   this [guide](https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver16&tabs=cli&pivots=cs1-bash)
4. Run migrations

## To obtain admin privileges

1. Create first account
2. Obtain admin privileges by setting the account email address in `CreateRolesAndUser` action in Home controller
3. Visit `localhost:{port}/Home/CreateRolesAndUser`
4. The account is now an admin