using System.Windows.Forms;

namespace Browser
{
	public class ToolStripOverride : ToolStripProfessionalRenderer
	{
		public ToolStripOverride()
		{
			this.RoundedEdges = false;
		}
		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) { }
	}
}
