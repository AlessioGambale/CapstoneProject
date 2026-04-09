
public enum SpecialAttackType 
{
    None,
    HeavyHit,        // colpo + 60% danno ma costo aumenta di 1AP (matita)
    IgnoreDefense,   // ignora una % di defence nemica (ago) 
    AOE,             // colpisce tutti i nemici e barra cresce a tutti (candy stick) 
    ApplyPanic,      // applica direttamente status Panic (righello rotto) 
    Execute          // attacco speciale che fa più danno se HP nemico basso(forbici) //???
}
