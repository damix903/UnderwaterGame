using EnemyAI.Move;
using Movement;

public class EnemyContext
{
    public EnemyData Data { get; private set; }
    //public IMoveable Moveable { get; private set; }
    //public IMoveable ChaseMoveable { get; private set; }
    public CharacterMovement Movement { get; private set; }
    public IAnimPlayable Anim { get; private set; }
    public IAnimEventListenable EventListenable { get; private set; }
    
    //public void SetMoveable(IMoveable moveable) => Moveable = moveable;
    //public void SetChaseMoveable(IMoveable moveable) => ChaseMoveable = moveable;
    
    public void SetData(EnemyData data) => Data = data;
    
    public class Builder
    {
        private EnemyContext _ctx = new EnemyContext();
        
        public Builder WithData(EnemyData data)
        {
            _ctx.Data = data;
            return this;
        }
        
        //public Builder WithMoveable(IMoveable moveable)
        //{
        //    _ctx.Moveable = moveable;
        //    return this;
        //}
        
        public Builder WithMovement(CharacterMovement movement)
        {
            _ctx.Movement = movement;
            return this;
        }
        
        public Builder WithAnim(IAnimPlayable anim)
        {
            _ctx.Anim = anim;
            return this;
        }
        
        public Builder WithEventListenable(IAnimEventListenable eventListenable)
        {
            _ctx.EventListenable = eventListenable;
            return this;
        }
        
        public EnemyContext Build()
        {
            return _ctx;
        }
    }
}