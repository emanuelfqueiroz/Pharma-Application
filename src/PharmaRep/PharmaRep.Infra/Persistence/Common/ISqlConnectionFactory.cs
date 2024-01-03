using Microsoft.Data.SqlClient;
using System.Data;

namespace PharmaRep.Infra.Persistence.Common
{
    public interface ISqlConnectionFactory
    {
        SqlConnection GetConnection();

        SqlCommand CreateCommand(string query);
    }
}