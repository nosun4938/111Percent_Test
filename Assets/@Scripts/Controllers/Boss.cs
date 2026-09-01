using System.Collections;
using UnityEngine;
using static Define;

public class Boss : Monster
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        StartCoroutine(CoUpdateAI());
        return true;
    }

    public float UpdateAITick { get; protected set; } = 3.0f;
    protected IEnumerator CoUpdateAI()
    {
        while (true)
        {
            if (Target.IsValid())
            {
                StartCoroutine(CoSpriteKnockback(Vector2.down));
                Debug.Log("Monster Skill Used");
            }

            if (UpdateAITick > 0)
                yield return new WaitForSeconds(UpdateAITick);
            else
                yield return null;
        }
    }

    public override void SetInfo(int templateID)
    {
        base.SetInfo(templateID);
    }

    #region Battle
    public override void OnDamaged(BaseObject attacker, SkillBase skill)
    {
        base.OnDamaged(attacker, skill);
    }

    public override void OnDead(BaseObject attacker, SkillBase skill)
    {
        base.OnDead(attacker, skill);
        int dropItemId = Random.Range(1, 11); // Monster Drop ID로 대체 가능

        Item item = Managers.Inventory.MakeItem(dropItemId);
        if (item == null)
        {
            Debug.Log("아이템 생성 실패");
            return;
        }
        Debug.Log($"아이템 획득 : {item.TemplateID}");
    }
    #endregion

    public IEnumerator CoSpriteKnockback(Vector3 dir)
    {
        Vector3 origin = transform.localPosition;
        Vector3 target = origin + dir.normalized * 2.0f;

        float duration = 0.3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / duration;
            transform.localPosition = Vector3.Lerp(origin, target, t);

            yield return null;
        }

        duration = 0.1f;
        elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / duration;
            transform.localPosition = Vector3.Lerp(target, origin, t);

            yield return null;
        }

        Skills.DefaultSkill.DoSkill();
        transform.localPosition = origin;
    }
}
