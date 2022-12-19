public class MoverBoss : Mover
{
    public override void SetDamage()
    {
        base.SetDamage();

        if (PlayerTarget == null || PlayerTarget.IsDead)
            return;

        if (PlayerTarget.CurrentState == State.Attack)
            Enemy.Attack(PlayerTarget);
    }
}