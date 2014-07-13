using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;


namespace Acr.Mail.Loaders {
    
    public class SqlMailTemplateLoader : IMailTemplateLoader {

        public string ConnectionString { get; set; }
        public string TableName { get; set; }


        public SqlMailTemplateLoader(string connectionString, string tableName = "dbo.MailTemplates") {
            this.ConnectionString = connectionString;
            this.TableName = tableName;
        }


        public IMailTemplate Load(string templateName, CultureInfo cultureInfo) {
            using (var connection = new SqlConnection(this.ConnectionString)) {
                connection.Open();
                using (var command = connection.CreateCommand()) {
                    command.CommandType = CommandType.Text;
                    command.CommandText = String.Format("SELECT LastModified, Content FROM {0} WHERE TemplateName = @Name AND Culture = @Culture", this.TableName);
                    command.Parameters.Add(new SqlParameter("@TemplateName", templateName));
                    command.Parameters.Add(new SqlParameter("@Culture", cultureInfo.Name));

                    using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection)) 
                        if (reader.Read()) 
                            return new SqlMailTemplate(reader.GetString(0), reader.GetDateTime(1), templateName, cultureInfo);        
                }
            }
            throw new ArgumentException("No template found");
        }
    }
}
