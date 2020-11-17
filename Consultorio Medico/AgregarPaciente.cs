using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Consultorio_Medico
{
    public partial class AgregarPaciente : Form
    {
        CRUD conection = new CRUD();
        Form1 form1;
        Boolean aceptar = false;

        public bool Aceptar { get => aceptar; set => aceptar = value; }

        public AgregarPaciente()
        {
            InitializeComponent();
            btnGuardar.Enabled = false;

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                String nombreYapellido = txtBoxNombreYapellido.Text;
                String dni = textBoxDni.Text;
                String direccion = textBoxDireccion.Text;
                String obraSocial = textBoxObraSocial.Text;
                String telefono = textBoxTelefono.Text;
                String email = textBoxEmail.Text;

                String consulta ="INSERT INTO paciente (nombreCompleto,dni,direccion,obraSocial,telefono,email) VALUES ('" + nombreYapellido + "','" + dni + "','" +direccion  + "','" + obraSocial + "','" + telefono + "','" + email + "')";
                conection.operaciones(consulta);
               
                MessageBox.Show("El paciente se agrego correctamente.");
                Aceptar = true;
               
            }
            catch(Exception)
            {
                MessageBox.Show("No se pudo cargar el paciente.");
            }


            this.Close();
            
        }

        private bool validarDatos()
        {
            if (txtBoxNombreYapellido.Text == "" || textBoxDireccion.Text=="" || textBoxDni.Text=="" || textBoxEmail.Text=="" || textBoxObraSocial.Text=="" || textBoxTelefono.Text=="")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void txtBoxNombreYapellido_TextChanged(object sender, EventArgs e)
        {
            btnGuardar.Enabled = validarDatos();
        }

        private void textBoxDni_TextChanged(object sender, EventArgs e)
        {
            btnGuardar.Enabled = validarDatos();

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
    }
}
