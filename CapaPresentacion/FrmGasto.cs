using System.Data;
using CapaNegocio;

namespace CapaPresentacion
{
    public class FrmGasto : Form
    {
        private GastoNegocio gastoNegocio = new GastoNegocio();
        private CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
        private int idSeleccionado = 0;

        private TextBox txtDescripcion;
        private TextBox txtMonto;
        private DateTimePicker dtpFecha;
        private ComboBox cboCategoria;
        private Button btnGuardar;
        private Button btnModificar;
        private Button btnEliminar;
        private Button btnLimpiar;
        private ComboBox cboMes;
        private NumericUpDown nudAnio;
        private Button btnFiltrar;
        private Button btnVerTodos;
        private DataGridView dgvGastos;
        private DataGridView dgvResumen;
        private Label lblMayor;

        private string[] meses = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                                   "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

        public FrmGasto()
        {
            ConstruirInterfaz();
            Load += FrmGasto_Load;
        }

        private Label CrearLabel(string texto, int x, int y)
        {
            Label lbl = new Label();
            lbl.Text = texto;
            lbl.Left = x;
            lbl.Top = y;
            lbl.AutoSize = true;
            Controls.Add(lbl);
            return lbl;
        }

        private Button CrearBoton(string texto, int x, int y, EventHandler evento)
        {
            Button btn = new Button();
            btn.Text = texto;
            btn.Left = x;
            btn.Top = y;
            btn.Width = 100;
            btn.Height = 30;
            btn.Click += evento;
            Controls.Add(btn);
            return btn;
        }

        private void ConstruirInterfaz()
        {
            Text = "Control de gastos personales";
            Width = 960;
            Height = 720;
            StartPosition = FormStartPosition.CenterScreen;

            // Datos del gasto
            CrearLabel("Descripción:", 20, 20);
            txtDescripcion = new TextBox();
            txtDescripcion.SetBounds(110, 17, 300, 25);
            Controls.Add(txtDescripcion);

            CrearLabel("Monto:", 440, 20);
            txtMonto = new TextBox();
            txtMonto.SetBounds(500, 17, 120, 25);
            Controls.Add(txtMonto);

            CrearLabel("Fecha:", 20, 60);
            dtpFecha = new DateTimePicker();
            dtpFecha.Format = DateTimePickerFormat.Short;
            dtpFecha.SetBounds(110, 57, 150, 25);
            Controls.Add(dtpFecha);

            CrearLabel("Categoría:", 290, 60);
            cboCategoria = new ComboBox();
            cboCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategoria.SetBounds(360, 57, 200, 25);
            Controls.Add(cboCategoria);

            btnGuardar = CrearBoton("Guardar", 20, 100, btnGuardar_Click);
            btnModificar = CrearBoton("Modificar", 130, 100, btnModificar_Click);
            btnEliminar = CrearBoton("Eliminar", 240, 100, btnEliminar_Click);
            btnLimpiar = CrearBoton("Limpiar", 350, 100, btnLimpiar_Click);

            // Filtro por mes y año
            CrearLabel("Mes:", 520, 108);
            cboMes = new ComboBox();
            cboMes.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMes.Items.AddRange(meses);
            cboMes.SetBounds(560, 105, 110, 25);
            Controls.Add(cboMes);

            CrearLabel("Año:", 685, 108);
            nudAnio = new NumericUpDown();
            nudAnio.Minimum = 2000;
            nudAnio.Maximum = 2100;
            nudAnio.SetBounds(725, 105, 70, 25);
            Controls.Add(nudAnio);

            btnFiltrar = CrearBoton("Filtrar", 805, 100, btnFiltrar_Click);
            btnFiltrar.Width = 60;
            btnVerTodos = CrearBoton("Todos", 870, 100, btnVerTodos_Click);
            btnVerTodos.Width = 60;

            // Lista de gastos
            dgvGastos = new DataGridView();
            dgvGastos.SetBounds(20, 145, 910, 260);
            dgvGastos.ReadOnly = true;
            dgvGastos.AllowUserToAddRows = false;
            dgvGastos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvGastos.MultiSelect = false;
            dgvGastos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvGastos.CellClick += dgvGastos_CellClick;
            Controls.Add(dgvGastos);

            // Resumen del mes
            CrearLabel("Total del mes por categoría:", 20, 420);
            dgvResumen = new DataGridView();
            dgvResumen.SetBounds(20, 445, 450, 180);
            dgvResumen.ReadOnly = true;
            dgvResumen.AllowUserToAddRows = false;
            dgvResumen.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Controls.Add(dgvResumen);

            CrearLabel("Gasto de mayor monto del mes:", 500, 420);
            lblMayor = new Label();
            lblMayor.SetBounds(500, 450, 430, 120);
            lblMayor.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            Controls.Add(lblMayor);
        }

        private void FrmGasto_Load(object sender, EventArgs e)
        {
            try
            {
                cboCategoria.DisplayMember = "Nombre";
                cboCategoria.ValueMember = "IdCategoria";
                cboCategoria.DataSource = categoriaNegocio.Listar();

                cboMes.SelectedIndex = DateTime.Today.Month - 1;
                nudAnio.Value = DateTime.Today.Year;

                ListarTodos();
                ActualizarResumen();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al iniciar: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarGastos(DataTable tabla)
        {
            dgvGastos.DataSource = tabla;
            dgvGastos.Columns["IdGasto"].Visible = false;
            dgvGastos.Columns["IdCategoria"].Visible = false;
            dgvGastos.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvGastos.Columns["Monto"].DefaultCellStyle.Format = "N2";
        }

        private void ListarTodos()
        {
            MostrarGastos(gastoNegocio.Listar());
        }

        private void ActualizarResumen()
        {
            int mes = cboMes.SelectedIndex + 1;
            int anio = Convert.ToInt32(nudAnio.Value);

            dgvResumen.DataSource = gastoNegocio.TotalMesPorCategoria(mes, anio);
            dgvResumen.Columns["Total"].DefaultCellStyle.Format = "N2";

            DataTable mayor = gastoNegocio.MayorGastoDelMes(mes, anio);
            if (mayor.Rows.Count > 0)
            {
                DataRow fila = mayor.Rows[0];
                lblMayor.Text = fila["Descripcion"].ToString() + Environment.NewLine +
                    "Monto: " + Convert.ToDecimal(fila["Monto"]).ToString("N2") + Environment.NewLine +
                    "Categoría: " + fila["Categoria"].ToString() + Environment.NewLine +
                    "Fecha: " + Convert.ToDateTime(fila["Fecha"]).ToString("dd/MM/yyyy");
            }
            else
            {
                lblMayor.Text = "No hay gastos en ese mes.";
            }
        }

        private void LimpiarCampos()
        {
            idSeleccionado = 0;
            txtDescripcion.Clear();
            txtMonto.Clear();
            dtpFecha.Value = DateTime.Today;
            if (cboCategoria.Items.Count > 0)
            {
                cboCategoria.SelectedIndex = 0;
            }
            dgvGastos.ClearSelection();
            txtDescripcion.Focus();
        }

        private void RefrescarTodo()
        {
            ListarTodos();
            ActualizarResumen();
            LimpiarCampos();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                gastoNegocio.Agregar(txtDescripcion.Text, txtMonto.Text, dtpFecha.Value, cboCategoria.SelectedValue);
                MessageBox.Show("Gasto guardado correctamente.", "Información",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefrescarTodo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                gastoNegocio.Modificar(idSeleccionado, txtDescripcion.Text, txtMonto.Text, dtpFecha.Value, cboCategoria.SelectedValue);
                MessageBox.Show("Gasto modificado correctamente.", "Información",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefrescarTodo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (idSeleccionado > 0)
                {
                    DialogResult respuesta = MessageBox.Show("¿Desea eliminar el gasto seleccionado?", "Confirmar",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (respuesta != DialogResult.Yes)
                    {
                        return;
                    }
                }
                gastoNegocio.Eliminar(idSeleccionado);
                MessageBox.Show("Gasto eliminado correctamente.", "Información",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefrescarTodo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                int mes = cboMes.SelectedIndex + 1;
                int anio = Convert.ToInt32(nudAnio.Value);
                MostrarGastos(gastoNegocio.ListarPorMesAnio(mes, anio));
                ActualizarResumen();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnVerTodos_Click(object sender, EventArgs e)
        {
            try
            {
                ListarTodos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvGastos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow fila = dgvGastos.Rows[e.RowIndex];
                    idSeleccionado = Convert.ToInt32(fila.Cells["IdGasto"].Value);
                    txtDescripcion.Text = fila.Cells["Descripcion"].Value.ToString();
                    txtMonto.Text = Convert.ToDecimal(fila.Cells["Monto"].Value).ToString("0.00");
                    dtpFecha.Value = Convert.ToDateTime(fila.Cells["Fecha"].Value);
                    cboCategoria.SelectedValue = Convert.ToInt32(fila.Cells["IdCategoria"].Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
