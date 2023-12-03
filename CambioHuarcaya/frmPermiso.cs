using CambioHuarcaya.Utilizable;
using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CambioHuarcaya
{
    public partial class frmPermiso : Form
    {
        public frmPermiso()
        {
            InitializeComponent();
        }

        private void frmPermiso_Load(object sender, EventArgs e)
        {

            List<Rol> ls = new CN_Rol().Listar();
            foreach (Rol item in ls)
            {
                cbUusario.Items.Add(new OpcionCombo()
                {
                    Valor= item.IdRol,
                    Text = item.Descripcion
                });
            }
            cbUusario.DisplayMember = "Text";
            cbUusario.ValueMember = "Valor";
            cbUusario.SelectedIndex = 0;

            setChecks();
        }

        private void cbUusario_SelectedIndexChanged(object sender, EventArgs e)
        {
            setChecks();
        }

        
        private List<bool> obtenerChecks()
        {
            List<bool> list = new List<bool>();
            list.Add(MENUUSARIO.Checked);
            list.Add(MENUMANTE.Checked);
            list.Add(MENUVEN.Checked);
            list.Add(MENUCOM.Checked);
            list.Add(MENICLIENT.Checked);
            list.Add(MENUPROVE.Checked);
            list.Add(MENURE.Checked);
            list.Add(MENUACER.Checked);

            return list;
        }
        private List<Permiso> ListarMenus()
        {
            List<Permiso> list = new List<Permiso>();
            list.Add(new Permiso() { NombreMenu = "MENUUSARIO" });
            list.Add(new Permiso() { NombreMenu = "MENUMANTE" });
            list.Add(new Permiso() { NombreMenu = "MENUVEN" });
            list.Add(new Permiso() { NombreMenu = "MENUCOM" });
            list.Add(new Permiso() { NombreMenu = "MENICLIENT" });
            list.Add(new Permiso() { NombreMenu = "MENUPROVE" });
            list.Add(new Permiso() { NombreMenu = "MENURE" });
            list.Add(new Permiso() { NombreMenu = "MENUACER" });

            return list;
        }

      
        private void setChecks()
        {
            int IdRol = Convert.ToInt32(((OpcionCombo)cbUusario.SelectedItem).Valor);

            // Obtén la lista de permisos para el rol actual
            List<Permiso> oPermisos = new CN_Permiso().ListarPermisos(IdRol);

            // Verifica si la lista no está vacía antes de acceder a los elementos
            if (oPermisos != null && oPermisos.Count > 0)
            {
                // Asegúrate de que hay suficientes elementos antes de acceder a ellos
                MENUUSARIO.Checked = oPermisos.Count > 0 && oPermisos[0].Estado;
                MENUMANTE.Checked = oPermisos.Count > 1 && oPermisos[1].Estado;
                MENUVEN.Checked = oPermisos.Count > 2 && oPermisos[2].Estado;
                MENUCOM.Checked = oPermisos.Count > 3 && oPermisos[3].Estado;
                MENICLIENT.Checked = oPermisos.Count > 4 && oPermisos[4].Estado;
                MENUPROVE.Checked = oPermisos.Count > 5 && oPermisos[5].Estado;
                MENURE.Checked = oPermisos.Count > 6 && oPermisos[6].Estado;
                MENUACER.Checked = oPermisos.Count > 7 && oPermisos[7].Estado;
            }

            txtIdRol.Text = IdRol.ToString();
        }

        private void BtnGuardarRol_Click(object sender, EventArgs e)
        {
            txtIdRol.Text = "0";
            int count = 0;
            int isLog = 0;
            string mensaje = string.Empty;
            string salida = string.Empty;
            int IdRolDevuelto = 0;
            Rol oRol = new Rol()
            {
                Descripcion = txtRol.Text.ToString().ToUpper()
            };
            int result = new CN_Rol().Registrar(oRol, out mensaje);
            if (result > 0)
            {
                IdRolDevuelto = new CN_Rol().IdRol();
                txtIdRol.Text = IdRolDevuelto.ToString();
                if (IdRolDevuelto == 0)
                {
                    return;
                }
                else
                {
                    List<Permiso> lsPermisos = ListarMenus();
                    foreach (Permiso oPermiso in lsPermisos)
                    {
                        Permiso _auxPermiso = new Permiso()
                        {
                            oRol = new Rol()
                            {
                                IdRol = Convert.ToInt32(txtIdRol.Text.ToString()),
                            },
                            NombreMenu = oPermiso.NombreMenu,
                            Estado = false
                        };
                        count++;
                        int permiso = new CN_Permiso().Registrar(_auxPermiso, out mensaje);
                        if (permiso > 0)
                            isLog++;
                    }
                }
                if (isLog >= 8)
                {
                    MessageBox.Show("Rol agregado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtRol.Text = "";
                    txtIdRol.Text = "0";

                    cbUusario.Items.Clear();
                    List<Rol> ls = new CN_Rol().Listar();
                    foreach (Rol item in ls)
                    {
                        cbUusario.Items.Add(new OpcionCombo()
                        {
                            Valor = item.IdRol,
                            Text = item.Descripcion
                        });
                    }
                    cbUusario.DisplayMember = "Text";
                    cbUusario.ValueMember = "Valor";
                    cbUusario.SelectedIndex = 0;

                }

            }
            else
            {
                MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void BtnGuardarPermiso_Click(object sender, EventArgs e)
        {
            int count = 0;
            int result = 0;
            string mensaje = string.Empty;
            int IdRol = Convert.ToInt32(((OpcionCombo)cbUusario.SelectedItem).Valor);
            var dialog =
                    MessageBox.Show("¿Seguro que quieres modificar los permisos de " + ((OpcionCombo)cbUusario.SelectedItem).Text + "?",
                    "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dialog == DialogResult.Yes)
            {
                txtIdRol.Text = IdRol.ToString();
                if (IdRol > 0)
                {
                    List<bool> lsCheck = obtenerChecks();
                    List<Permiso> lsPermisos = ListarMenus();
                    foreach (Permiso oPermiso in lsPermisos)
                    {
                        Permiso _auxPermiso = new Permiso()
                        {
                            oRol = new Rol()
                            {
                                IdRol = Convert.ToInt32(txtIdRol.Text.ToString()),
                            },
                            NombreMenu = oPermiso.NombreMenu,
                            Estado = lsCheck[count]
                        };
                        count++;
                        result = new CN_Permiso().Editar(_auxPermiso, out mensaje);
                    }
                    MessageBox.Show("Permisos actualizados", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                cbUusario.SelectedIndex = 0;
            }
            else
            {
                setChecks();
            }
        }
    }
    
}
