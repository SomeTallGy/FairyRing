using GameSparks.Core;

namespace FairyO.World
{
    public abstract class NodeObject : INodeObject
    {
        // -------- enums -----------
        public enum Type { Gatherable, Creature };

        // -------- properties -------
		public string name {get; private set;}
        public string asset {get; private set;}
        public GSData data {get; private set;}

		public virtual void Init(GSData data)
		{
			this.name = data.GetString("name");
            this.asset = data.GetString("asset");
			
            this.data = data; // store data 
		}
    }

}