using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Media.Media3D;

namespace LINQ2
{
    // Practica de actualizacion de base de datos con distintos metodos por medio de lambda.
    public partial class MainWindow : Window
    {
        //Utilizando nuestro archivo DataClasses1.dbml
        DataClasses1DataContext dataContext;

        public MainWindow()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LINQ2.Properties.Settings.EscDirectaDBConnectionString"].ConnectionString;
            dataContext = new DataClasses1DataContext(connectionString);

            InitializeComponent();

            //AgregarUniversidad();
            //AgregarEstudiante();
            //AgregarMateria();
            //AgregarAsociacionesMateria();
            //ObtenerMaterias();
            //ObtenerUniversidadesFemeninas();
            //ObtenerMateriasUBA();
            //ActualizarLautaro();
            //ObtenerEstudiantesDeUNC();
            //EliminarXimena();

        }

        //Metodo para eliminar un estudiante de la base de datos
        private void EliminarXimena()
        {
            var eliminarJuan = dataContext.Estudiante.First(es => es.Nombre == "Ximena");

            dataContext.Estudiante.DeleteOnSubmit(eliminarJuan);
            dataContext.SubmitChanges();
            DataGridPrincipal.ItemsSource = dataContext.Estudiante;

        }
        //Actualizar informacion de nuestra base de datos
        private void ActualizarLautaro()
        {

            Estudiante Lautaro = dataContext.Estudiante.FirstOrDefault(es => es.Nombre == "Lautaro");

            Lautaro.Nombre = "Lautarino";
            dataContext.SubmitChanges();
            DataGridPrincipal.ItemsSource = dataContext.Estudiante;
        }

        //Obtenemos la informacion de las materias que se imparten en la universidad UBA
        private void ObtenerMateriasUBA()
        {
            var materias = from em in dataContext.MateriaEstudiante
                           join estudiante in dataContext.Estudiante
                           on em.EstudianteId equals estudiante.Id
                           where estudiante.Universidad.Nombre == "UBA"
                           select em.Materia;
            DataGridPrincipal.ItemsSource = materias;
        }
        private void ObtenerMateriasUNC()
        {
            var materias = from em in dataContext.MateriaEstudiante
                           join estudiante in dataContext.Estudiante
                           on em.EstudianteId equals estudiante.Id
                           where estudiante.Universidad.Nombre == "UNC"
                           select em.Materia;
            DataGridPrincipal.ItemsSource = materias;
        }

        //Estudiantes que participan en la universidad UNC
        public void ObtenerEstudiantesDeUNC()
        {
            var estudiantesUNC = from estudiante in dataContext.Estudiante where estudiante.Universidad.Nombre == "UNC" select estudiante;

            DataGridPrincipal.ItemsSource = estudiantesUNC;
        }
        //Obtener estudiantes de genero femenino
        private void ObtenerUniversidadesFemeninas()
        {
            var estudiantesFemeninas = from estudiante in dataContext.Estudiante
                                       join universidad in dataContext.Universidad on estudiante.Universidad
                                       equals universidad
                                       where estudiante.Genero == "Femenino"
                                       select universidad;
            DataGridPrincipal.ItemsSource = estudiantesFemeninas;
        }
        private void ObtenerEstudiantes()
        {
            var estudiantesUNC = from estudiante in dataContext.Estudiante where estudiante.Universidad.Nombre == "UNC" select estudiante;
            DataGridPrincipal.ItemsSource = estudiantesUNC;
        }
        private void ObtenerMaterias()
        {
            Estudiante mateo = dataContext.Estudiante.First(es => es.Nombre.Equals("Lautaro"));

            var materiasMateo = from em in mateo.MateriaEstudiante select em.Materia;

            DataGridPrincipal.ItemsSource = materiasMateo;
        }

        //Insertar en EstudianteMateria enlazando las dos tablas
        private void AgregarAsociacionesMateria()
        {

            Estudiante Lautaro = dataContext.Estudiante.First(es => es.Nombre.Equals("Lautaro"));
            Estudiante Ximena = dataContext.Estudiante.First(es => es.Nombre.Equals("Ximena"));
            Estudiante Raul = dataContext.Estudiante.First(es => es.Nombre.Equals("Raul"));

            Materia Matematica = dataContext.Materia.First(es => es.Nombre.Equals("Matematica"));
            Materia Lenguaje = dataContext.Materia.First(es => es.Nombre.Equals("Lenguaje"));
            Materia Biologia = dataContext.Materia.First(es => es.Nombre.Equals("Biologia"));

            dataContext.MateriaEstudiante.InsertOnSubmit(new MateriaEstudiante { Estudiante = Lautaro, Materia = Matematica });
            dataContext.MateriaEstudiante.InsertOnSubmit(new MateriaEstudiante { Estudiante = Lautaro, Materia = Lenguaje });
            dataContext.MateriaEstudiante.InsertOnSubmit(new MateriaEstudiante { Estudiante = Raul, Materia = Lenguaje });

            dataContext.SubmitChanges();
            DataGridPrincipal.ItemsSource = dataContext.MateriaEstudiante;
        }

        //Insertar las distintas tablas y listas con submit y submit all
        private void AgregarMateria()
        {
            List<Materia> Materias = new List<Materia>();

            Materias.Add(new Materia { Nombre = "Lenguaje" });
            Materias.Add(new Materia { Nombre = "Matematica" });
            Materias.Add(new Materia { Nombre = "Biologia" });

            foreach (Materia i in Materias)
            {
                dataContext.Materia.InsertOnSubmit(i);
                dataContext.SubmitChanges();
            }
            DataGridPrincipal.ItemsSource = dataContext.Materia;
        }
        private void AgregarEstudiante()
        {
            Universidad UNC = dataContext.Universidad.First(un => un.Nombre.Equals("UNC"));
            Universidad UBA = dataContext.Universidad.First(un => un.Nombre.Equals("UBA"));

            List<Estudiante> estudiantes = new List<Estudiante>();

            estudiantes.Add(new Estudiante { Nombre = "Raul", Genero = "Masculino", UniversidadId = UNC.Id });
            estudiantes.Add(new Estudiante { Nombre = "Ximena", Genero = "Femenino", UniversidadId = UBA.Id });
            estudiantes.Add(new Estudiante { Nombre = "Mateo", Genero = "Masculino", Universidad = UNC });
            estudiantes.Add(new Estudiante { Nombre = "Lautaro", Genero = "Masculino", Universidad = UBA });
            estudiantes.Add(new Estudiante { Nombre = "Laura", Genero = "Femenino", Universidad = UBA });

            dataContext.Estudiante.InsertAllOnSubmit(estudiantes);
            dataContext.SubmitChanges();

            DataGridPrincipal.ItemsSource = dataContext.Estudiante;


        }
        private void AgregarUniversidad()
        {
            dataContext.ExecuteCommand("Delete from Universidad");
            Universidad UNC = new Universidad();
            Universidad UBA = new Universidad();

            UNC.Nombre = "UNC";
            UBA.Nombre = "UBA";

            dataContext.Universidad.InsertOnSubmit(UNC);
            dataContext.Universidad.InsertOnSubmit(UBA);

            dataContext.SubmitChanges();

            DataGridPrincipal.ItemsSource = dataContext.Universidad;

        }
    }
}
