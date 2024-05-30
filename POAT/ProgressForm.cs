using System;
using System.Drawing;
using System.Windows.Forms;

namespace POAT
{
    public partial class ProgressForm : Form
    {
        private Form parentForm;
        private ProgressBar progressBar;

        public ProgressForm(Form parent)
        {
            InitializeComponent();
            this.parentForm = parent; // Stockez la référence au formulaire principal
            this.StartPosition = FormStartPosition.Manual; // Définissez le démarrage manuel
            CenterToParentForm(); // Centrez initialement la fenêtre de progression

            // Attachez l'événement de déplacement du formulaire principal pour re-centrer
            this.parentForm.Move += ParentFormMoved;
        }

        private void ParentFormMoved(object sender, EventArgs e)
        {
            CenterToParentForm(); 
        }

        private void CenterToParentForm()
        {
            // Calculez la nouvelle position centrée
            this.Location = new Point(
                parentForm.Location.X + (parentForm.Width - this.Width) / 2,
                parentForm.Location.Y + (parentForm.Height - this.Height) / 2
            );
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            this.parentForm.Move -= ParentFormMoved;
        }

        // Set la valeur de la ProgressBar
        public void SetProgress(int progress)
        {
            if (this.progressBar.InvokeRequired)
            {
                this.progressBar.Invoke(new Action(() => SetProgress(progress)));
            }
            else
            {
                this.progressBar.Value = progress;
            }
        }

        // Fermer le formulaire ProgressForm
        public void CloseForm()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(CloseForm));
            }
            else
            {
                this.Close();
                Application.OpenForms[0].BringToFront();
            }
        }

    }
}
