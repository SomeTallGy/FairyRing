using FairyO.Spatial;

namespace FairyO.World
{
	public interface IWorldObject {
		int id {get;}
		WorldSpatial spatial {get;}
		void Activate();
	}
}