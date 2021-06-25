using System;
using ConexionMYSQL;
using System.Windows.Forms;
using System.Collections.Generic;

namespace VeterDates {
    public partial class DetallesConsulta : Form {
        readonly BaseDatos @base = new BaseDatos("mysql-veterdates.alwaysdata.net", "237436", "Ma-^VdUyZeN$JyW", "veterdates_tijuana");
        readonly Form menuPrincipal;
        public DetallesConsulta( Form menu ) {
            InitializeComponent();
            this.menuPrincipal = menu;
            crearGrid();
            cargarDatos();
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.bunifuImageButton1.Hide();
            this.bunifuImageButton2.Hide();
            this.bunifuImageButton4.Hide();
        }
        public DetallesConsulta( Form menu , string idCita): this(menu) {
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDown;
            this.comboBox1.Text = idCita;
            this.comboBox1.Enabled = false;
            this.bunifuImageButton1.Show();
            this.bunifuImageButton3.Hide();
        }

        private void pictureBox3_Click( object sender, EventArgs e ) {
            Application.Exit();
        }

        private void bunifuImageButton5_Click( object sender, EventArgs e ) {
            this.menuPrincipal.Show();
            Dispose();
        }
        private void limpiar( ) {
            this.comboBox1.Items.Clear();
            this.comboBox2.Items.Clear();
            this.comboBox3.Items.Clear();
            cargarDatos();
        }

        void cargarDatos( ) {
            this.bunifuDataGridView1.Rows.Clear();
            foreach (List<string> dt in this.@base.Buscar("DETALLESCONSULTA_debil")) {
                this.comboBox1.Items.Add(dt[0]);
                this.comboBox2.Items.Add(dt[ 1 ]);
                this.comboBox3.Items.Add(dt[ 2 ]);
                this.bunifuDataGridView1.Rows.Add(dt[ 0 ], dt[ 1 ], dt[ 2 ], dt[ 3 ].Remove(10), dt[ 4 ]);
            }
        }
        void crearGrid( ) {
            this.bunifuDataGridView1.ColumnCount = 5;
            this.bunifuDataGridView1.Columns[ 0 ].Name = "CONSULTA";
            this.bunifuDataGridView1.Columns[ 1 ].Name = "CONSULTORIO";
            this.bunifuDataGridView1.Columns[ 2 ].Name = "MASCOTA";
            this.bunifuDataGridView1.Columns[ 3 ].Name = "DIA";
            this.bunifuDataGridView1.Columns[ 4 ].Name = "HORA";
        }

        private void bunifuImageButton1_Click( object sender, EventArgs e ) {
            string[] datos = new string[] {
                $"('{this.comboBox1.Text}')",
                $"('{this.comboBox2.Text}')",
                $"('{this.comboBox3.Text}')",
                $"('{this.bunifuDatePicker1.Value:yyyy-MM-dd}')",
                $"('{this.dateTimePicker1.Text}')"
            };
            if (validos())
                MessageBox.Show("Los campos no deben estar vacios");
            else if (this.@base.Alta("DETALLESCONSULTA_debil", datos)) {
                MessageBox.Show("Registro completado");
                limpiar();
            }
            else if (MessageBox.Show("¿Desea sobreescribir los datos del ID?", "ID repetido", MessageBoxButtons.YesNo) == DialogResult.Yes)
                bunifuImageButton4_Click(null, null);
            else
                limpiar();
        }

        private void bunifuImageButton3_Click( object sender, EventArgs e ) {
            List<List<string>> consulta = this.@base.Buscar("DETALLESCONSULTA_debil", $"idConsulta='{this.comboBox1.Text}'");
            this.comboBox2.Text = consulta[ 0 ][ 1 ];
            this.comboBox3.Text = consulta[ 0 ][ 2 ];
            this.bunifuDatePicker1.Text = consulta[ 0 ][ 3 ];
            this.dateTimePicker1.Text = consulta[ 0 ][ 4 ];

            if (!string.IsNullOrEmpty(this.comboBox1.Text)) {
                this.comboBox1.Enabled = false;
                this.bunifuImageButton1.Hide();
                this.bunifuImageButton2.Hide();
                this.bunifuImageButton3.Hide();
                this.bunifuImageButton4.Show();
            }
        }

        private void bunifuImageButton4_Click( object sender, EventArgs e ) {
            string[] datos = new string[] {
                $"idConsultorio='{this.comboBox2.Text}'",
                $"idMascota='{this.comboBox3.Text}'",
                $"diaConsulta='{this.bunifuDatePicker1.Value:yyyy-MM-dd}'",
                $"horaConsulta='{this.dateTimePicker1.Text}'"
            };
            if (validos())
                MessageBox.Show("Los campos no deben estar vacios");
            else if (this.@base.Actualizar("DETALLESCONSULTA_debil", datos, $"idConsulta='{this.comboBox1.Text}'")) {
                MessageBox.Show("Registro actualizado");
                limpiar();
            }
            else
                MessageBox.Show("Ocurrió un error, revisa tus datos");
            this.comboBox1.Enabled = true;
            this.bunifuImageButton4.Hide();
            this.bunifuImageButton3.Show();
        }
        private bool validos( ) => 
            string.IsNullOrEmpty(this.comboBox1.Text) && 
            string.IsNullOrEmpty(this.comboBox2.Text) && 
            string.IsNullOrEmpty(this.comboBox3.Text);
    }
}
