using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MensajeMadre
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SqlConnection sqlConnection = new SqlConnection("Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = C:\\Users\\jimif\\Documents\\Code\\DessarrolloSoftwareIII\\MensajeMadre\\DataMadres.mdf; Integrated Security = True");
            sqlConnection.Open();
            SqlCommand cmd = sqlConnection.CreateCommand();
            SqlTransaction tran = null;

            Console.WriteLine("APLICACION MENSAJERIA MADRES");
            Console.WriteLine();

            int codigo;
            string descripcion, mensajeCorto, estado,
                mensajeLargo, enviadoPor, enviadoPara, parentesco;

            Console.WriteLine("Digite su codigo: ");
            codigo = int.Parse(Console.ReadLine());
            Console.WriteLine("Introduzca una descripcion del asunto: ");
            descripcion = Console.ReadLine();
            Console.WriteLine("Introduzca quien lo envia: ");
            enviadoPor = Console.ReadLine();
            Console.WriteLine("Introduzca para quien es: ");
            enviadoPara = Console.ReadLine();
            Console.WriteLine("Introduzca el mensaje corto: ");
            mensajeCorto = Console.ReadLine();
            Console.WriteLine("Introduzca el mensaje largo: ");
            mensajeLargo = Console.ReadLine();
            Console.WriteLine("Introduzca el parentesco con la persona: ");
            parentesco = Console.ReadLine();
            Console.WriteLine("Introduzca el estado del mensaje: ");
            estado = Console.ReadLine();

            try
            {
                cmd.CommandText = "ppUpsertPrincipal";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@codigo", codigo);
                cmd.Parameters.AddWithValue("@descripcion", descripcion);

                tran = sqlConnection.BeginTransaction();
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("@codigo", codigo);
                cmd.Parameters.AddWithValue("@mensajeCorto", mensajeCorto);
                cmd.Parameters.AddWithValue("@mensaje", mensajeLargo);
                cmd.Parameters.AddWithValue("@enviadoPor", enviadoPor);
                cmd.Parameters.AddWithValue("@enviadoPara", enviadoPara);
                cmd.Parameters.AddWithValue("@estado", estado);
                cmd.Parameters.AddWithValue("@parentesco", parentesco);

                cmd.CommandText = "ppIpsertDetalle";
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                tran.Commit();
            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
        }
    }
}
