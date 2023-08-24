using UnityEngine;

namespace _Game
{
    public interface ICollectible
    {
        public string Type { get; }
        public int Value { get; }
        public void Collect(Transform collector);
    }
}
