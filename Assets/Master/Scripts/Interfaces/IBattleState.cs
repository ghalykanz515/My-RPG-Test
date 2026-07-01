using System.Collections;

namespace RPGTest.Interfaces 
{
    public interface IBattleState
    {
        IEnumerator Enter();
        IEnumerator Exit();
    }
}