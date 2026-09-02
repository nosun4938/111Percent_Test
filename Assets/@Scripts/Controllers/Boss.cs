using System.Collections;
using UnityEngine;
using static Define;

public class Boss : Monster
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        
        return true;
    }

    public override void SetInfo(int templateID)
    {
        base.SetInfo(templateID);
        StartCoroutine(CoUpdateAI());
    }

    public float UpdateAITick { get; protected set; } = 3.0f;
    protected IEnumerator CoUpdateAI()
    {
        while (true)
        {
            int randSkill = Random.Range(0, Skills.SkillList.Count);
            SkillBase skill = Skills.SkillList[randSkill];

            if (Target.IsValid())
            {
                StartCoroutine(CoBossAttack(Vector2.down, skill));
            }

            if (UpdateAITick > 0)
                yield return new WaitForSeconds(UpdateAITick);
            else
                yield return null;
        }
    }

    #region Battle
    public override void OnDamaged(BaseObject attacker, SkillBase skill)
    {
        base.OnDamaged(attacker, skill);
    }

    public override void OnDead(BaseObject attacker, SkillBase skill)
    {
        base.OnDead(attacker, skill);

        // Temp Reward Data 필요
        int dropItemId = MonsterData.DropItemId; // 기본값, null 안나오게 설정해야함
        int randomInt = Random.Range(1, 101);

        if (randomInt == 100)
            dropItemId = 10;
        else
        {
            randomInt /= 10;
            dropItemId = randomInt == 0 ? 1 : randomInt;
        }

        if (Managers.Inventory.HasItem(dropItemId))
        {
            Debug.Log("이미 보유한 아이템, 골드로 대체");
            Managers.Game.EarnGold(Random.Range(50, 60));
            return;
        }

        Item item = Managers.Inventory.MakeItem(dropItemId);
        if (item == null)
        {
            Debug.Log("아이템 생성 실패");
            return;
        }

        Debug.Log($"아이템 획득 : {item.TemplateID}");
    }
    #endregion

    public IEnumerator CoBossAttack(Vector3 dir, SkillBase skill)
    {
        if (skill is Shield)
        {
            Skills.BSkill.DoSkill();
        }

        if (skill is BossAttack)
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
}
