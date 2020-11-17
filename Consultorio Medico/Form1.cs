using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Consultorio_Medico
{
    
    public partial class Form1 : Form
    {
        MySqlConnection connection = new MySqlConnection("Server=localhost; Database=consultoriodb; Uid=root; Pwd=''");
        MySqlDataAdapter adapter;
        CRUD conexion = new CRUD();
        DataTable table = new DataTable();
        String nombre;
        public static bool pacienteOk=false;
        public Form1()
        {
            InitializeComponent();
            conexion.Conexion();
            LoadListaPacientes();
        }

        private void textBoxBuscarPaciente_TextChanged(object sender, EventArgs e)
        {
            if (textBoxBuscarPaciente.Text!="" && textBoxBuscarPaciente.Text != null)
            {
                lstPacientes.DataSource = null;
                adapter = new MySqlDataAdapter("SELECT * FROM paciente WHERE dni = '" + textBoxBuscarPaciente.Text + "'", connection);
                table.Clear();
                adapter.Fill(table);
                lstPacientes.DataSource = table;
                lstPacientes.DisplayMember = "nombreCompleto";
                lstPacientes.ValueMember = "idPaciente";
            }
            else
            {
                LoadListaPacientes();
            }

            //Console.WriteLine("" + textBoxBuscarPaciente.Text);
            
        }

        private void textBoxBuscarPaciente_Click(object sender, EventArgs e)
        {
            textBoxBuscarPaciente.ReadOnly = false;
        }

        //Procedimiento para cargar la lista de los pacientes
        private void LoadListaPacientes()
        {
            
            lstPacientes.DataSource = null;
            adapter = new MySqlDataAdapter("SELECT * FROM paciente", connection);
            table.Clear();
            adapter.Fill(table);
            lstPacientes.DataSource = table;
            lstPacientes.DisplayMember = "nombreCompleto";
            lstPacientes.ValueMember = "idPaciente";
            if (lstPacientes.Items.Count == 0)
            {
                deshabilitarBotones(false);
            }
            else
            {
                deshabilitarBotones(true);
            }
        }

        //Procedimiento para mostrar los datos de los pacientes cuando se selecciona en la lista. 
        public void lstPacientes_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                
               // btnGuardarHC.Visible = false;
                textBoxHC.Text = "";
                //Datos del paciente
                String con = "SELECT * FROM paciente WHERE idPaciente = @valor";
                //devuelve un array
                DataTable Paciente = conexion.ConsultaParametrizada(con, lstPacientes.SelectedValue);
                nombre = Paciente.Rows[0].ItemArray[1].ToString();
                String dni = Paciente.Rows[0].ItemArray[2].ToString();
                String obraSocial = Paciente.Rows[0].ItemArray[4].ToString();
                String telefono = Paciente.Rows[0].ItemArray[5].ToString();
                String email = Paciente.Rows[0].ItemArray[6].ToString();
                String direccion = Paciente.Rows[0].ItemArray[3].ToString();
                textBoxDni.Text = dni;
                textBoxObraSocial.Text = obraSocial;
                textBoxTelefono.Text = telefono;
                textBoxEmail.Text = email;
                textBoxDireccion.Text = direccion;

                //historia clinica
                //Datos del paciente
                String con3 = "SELECT COUNT(*) FROM historiaclinica WHERE FK_idPaciente = '" + lstPacientes.SelectedValue + "'";
                String existeOno = conexion.ValorEnVariable(con3);
                if (int.Parse(existeOno) == 0)
                {
                    textBoxFechaModificacion.Text = "Sin fecha";
                    textBoxHC.Text = "El paciente "+ nombre + " no tiene historia clínica";
                }
                else
                {
                    
                    String con2 = "SELECT * FROM historiaclinica WHERE FK_idPaciente = @valor";
                    //devuelve un array
                    DataTable Paciente2 = conexion.ConsultaParametrizada(con2, lstPacientes.SelectedValue);
                    String fecha = Paciente2.Rows[0].ItemArray[1].ToString();
                    String info = Paciente2.Rows[0].ItemArray[2].ToString();
                    btnGuardarHC.Enabled = false;
                    textBoxFechaModificacion.Text = "";
                    textBoxFechaModificacion.Text = "Ultima modificacion: " + fecha;
                    textBoxHC.Text = info;
                }

                //Desabilitamos los campos para que solo se puedan leer y no volver a escribir
                deshabilitarModificaciones();

            }
            catch (Exception)
            {

             
            }

            //Si no hay pacientes cargados deshabilitamos botones y limpiamos campos
            if(lstPacientes.DataSource== null)
            {
                
                textBoxDni.Text = "";
                textBoxDireccion.Text = "";
                textBoxTelefono.Text = "";
                textBoxEmail.Text = "";
                textBoxObraSocial.Text = "";
            }
        }

        private void lstPacientes_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        
        

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBoxHC_TextChanged(object sender, EventArgs e)
        {
            
            
        }

        private void textBoxHC_ModifiedChanged(object sender, EventArgs e)
        {
           
              //  btnGuardarHC.Visible = true;
            
        }

        private void btnAgregarPaciente_Click(object sender, EventArgs e)
        {
           
            var formularioAgregarPaciente = new AgregarPaciente();

            //Abrimos el formulario para dar de alta a un nuevo paciente
            formularioAgregarPaciente.ShowDialog();

            if (formularioAgregarPaciente.Aceptar == true)
            {
                LoadListaPacientes();
            }
            
        }

        private void btnGuardarHC_Click(object sender, EventArgs e)
        {

            try
            {
                String con = "SELECT COUNT(*) FROM historiaclinica WHERE FK_idPaciente = '" + lstPacientes.SelectedValue + "'";
                String existeOno = conexion.ValorEnVariable(con);
                if (int.Parse(existeOno) == 0)
                {
                    //No tiene HC, entonces hacemos un insert
                    String consulta2 = "INSERT INTO historiaclinica (fecha,informacion,FK_idPaciente) VALUES ('" + DateTime.Today + "','"+textBoxHC.Text+"', '"+lstPacientes.SelectedValue+"')";
                    conexion.operaciones(consulta2);
                    MessageBox.Show("La historia clinica se guardo exitosamente.");
                }
                else
                {
                    //Tiene HC entonces hacemos update
                    String consulta = "UPDATE historiaclinica SET fecha = '" + DateTime.Today + "', informacion = '" + textBoxHC.Text + "' WHERE FK_idPaciente = '" + lstPacientes.SelectedValue + "'";
                    conexion.operaciones(consulta);
                    MessageBox.Show("La historia clinica se modificó exitosamente.");
                    btnGuardarHC.Enabled = false;
                }

               
            }
            catch (Exception)
            {
                MessageBox.Show("No se pudo guardar la historia clinica.");
                btnGuardarHC.Enabled = false;
            }
  
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnGuardarHC.Enabled = false;
        }

        private void textBoxHC_Click(object sender, EventArgs e)
        {
            textBoxHC.ReadOnly = false;
            btnGuardarHC.Enabled = true;
        }

        private void btnModificarDatosPaciente_Click(object sender, EventArgs e)
        {
            //Habilitamos los campos
            textBoxDni.ReadOnly = false;
            textBoxDireccion.ReadOnly = false;
            textBoxObraSocial.ReadOnly = false;
            textBoxTelefono.ReadOnly = false;
            textBoxEmail.ReadOnly = false;

        }

      
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            
            try
            {
                //Guardamos en variables los campos con cada dato
                String dni = textBoxDni.Text;
                String direccion = textBoxDireccion.Text;
                String obraSocial = textBoxObraSocial.Text;
                String telefono = textBoxTelefono.Text;
                String email = textBoxEmail.Text;

                //Hacemos el update de los campos
                String consulta = "UPDATE paciente SET dni= '" + dni + "', direccion= '" + direccion + "', obraSocial= '" + obraSocial + "',telefono='" + telefono + "',email='" + email + "' WHERE idPaciente = '" +lstPacientes.SelectedValue+"'";
                conexion.operaciones(consulta);
                MessageBox.Show("Los datos del paciente fueron actualizados.", "Exito");

                //Desabilitamos los campos para que solo se puedan leer y no volver a escribir
                deshabilitarModificaciones();



            }
            catch (Exception)
            {
                //No se actualiza
                MessageBox.Show("No se pudo actualizar los datos del paciente.", "ERROR");
                //Desabilitamos los campos para que solo se puedan leer y no volver a escribir
                deshabilitarModificaciones();


            }
        }

        private void deshabilitarModificaciones()
        {
            //Desabilitamos los campos para que solo se puedan leer y no volver a escribir
            textBoxDni.ReadOnly = true;
            textBoxDireccion.ReadOnly = true;
            textBoxObraSocial.ReadOnly = true;
            textBoxTelefono.ReadOnly = true;
            textBoxEmail.ReadOnly = true;
        }

        #region validar campos modificados
        private bool validarDatos()
        {
            if (textBoxDni.Text == "" || textBoxDireccion.Text == "" || textBoxObraSocial.Text == "" || textBoxEmail.Text == "" || textBoxTelefono.Text == "")
            {
                btnGuardar.Enabled = false;
                return false;
            }
            else
            {
                btnGuardar.Enabled = true;
                return true;
            }
        }

        private void textBoxObraSocial_TextChanged(object sender, EventArgs e)
        {
            btnGuardar.Enabled = validarDatos();
        }

        private void textBoxTelefono_TextChanged(object sender, EventArgs e)
        {
            btnGuardar.Enabled = validarDatos();
        }

        private void textBoxEmail_TextChanged(object sender, EventArgs e)
        {
            btnGuardar.Enabled = validarDatos();
        }

        private void textBoxDireccion_TextChanged(object sender, EventArgs e)
        {
            btnGuardar.Enabled = validarDatos();
        }
        private void textBoxDni_TextChanged(object sender, EventArgs e)
        {
            btnGuardar.Enabled = validarDatos();
        }
        #endregion

        private void btnEliminarPaciente_Click(object sender, EventArgs e)
        {

            DialogResult r=MessageBox.Show("Esta seguro que desea eliminar el paciente " +nombre + "?.", "ATENCION",MessageBoxButtons.YesNo);

            if (r==DialogResult.Yes)
            {
                try
                {
                    String consulta2 = "DELETE FROM historiaclinica WHERE FK_idPaciente = '" + lstPacientes.SelectedValue + "'";
                    conexion.operaciones(consulta2);
                    String consulta = "DELETE FROM paciente WHERE idPaciente = '" + lstPacientes.SelectedValue + "'";
                    conexion.operaciones(consulta);

                    LoadListaPacientes();
                    MessageBox.Show("Se elimino el usuario correctamente.");
                }
                catch (Exception)
                {
                    MessageBox.Show("No se pudo eliminar el usuario.");
                }
                
            }
            else
            {
               
            }

           
        }

        private void deshabilitarBotones(bool bandera)
        {
            btnGuardar.Enabled = bandera;
            btnEliminarPaciente.Enabled = bandera;
            btnModificarDatosPaciente.Enabled = bandera;
            textBoxHC.Enabled = bandera;
            textBoxBuscarPaciente.Enabled = bandera;

        }
    }
}
