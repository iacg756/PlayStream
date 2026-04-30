using System.Data;
using PlayStream.Core.Enum;
namespace PlayStream.Core.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
        DataBaseProvider Provider { get; }
    }
}