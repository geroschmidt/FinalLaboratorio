using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consultorio_Medico
{
    class Paciente
    {
        private String nombreCompleto;
        private String dni;
        private String direccion;
        private String obraSocial;
        private String telefono;
        private String email;

        public Paciente(string nombreCompleto, string dni, string direccion, string obraSocial, string telefono, string email)
        {
            this.nombreCompleto = nombreCompleto;
            this.dni = dni;
            this.direccion = direccion;
            this.obraSocial = obraSocial;
            this.telefono = telefono;
            this.email = email;

        }

        public string Nombre { get => nombreCompleto; set => nombreCompleto = value; }
        public string Dni { get => dni; set => dni = value; }
        public string Direccion { get => direccion; set => direccion = value; }
        public string ObraSocial { get => obraSocial; set => obraSocial = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public string Email { get => email; set => email = value; }
    }
}
