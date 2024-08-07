﻿using System;
using AAEmu.Commons.Network;
using AAEmu.Game.Core.Managers;
using AAEmu.Game.Models.Game.Items.Templates;

namespace AAEmu.Game.Models.Game.Items;

public class EquipItem : Item
{
    public override ItemDetailType DetailType => ItemDetailType.Equipment;
    public override uint DetailBytesLength => 55;

    public byte Durability { get; set; }
    public uint RuneId { get; set; }
    public uint[] GemIds { get; set; }
    public ushort TemperPhysical { get; set; }
    public ushort TemperMagical { get; set; }

    public virtual int Str => 0;
    public virtual int Dex => 0;
    public virtual int Sta => 0;
    public virtual int Int => 0;
    public virtual int Spi => 0;
    public virtual byte MaxDurability => 0;

    /// <summary>
    /// The item ID of the dye pot that was used on the equipment.
    /// </summary>
    public uint DyeItemId { get; set; }

    public int RepairCost
    {
        get
        {
            var template = (EquipItemTemplate)Template;
            var grade = ItemManager.Instance.GetGradeTemplate(Grade);
            var cost = (ItemManager.Instance.GetDurabilityRepairCostFactor() * 0.0099999998f) *
                       (1f - Durability * 1f / MaxDurability) * template.Price;
            cost = cost * grade.RefundMultiplier * 0.0099999998f;
            cost = (float)Math.Ceiling(cost);
            if (cost < 0 || cost < int.MinValue || cost > int.MaxValue)
                cost = 0;
            return (int)cost;
        }
    }

    public EquipItem()
    {
        GemIds = new uint[7];
    }

    public EquipItem(ulong id, ItemTemplate template, int count) : base(id, template, count)
    {
        GemIds = new uint[7];
        DyeItemId = ((EquipItemTemplate)Template).DefaultDyeItemId;
    }

    public override void Read(PacketStream stream)
    {
        TemplateId = stream.ReadUInt32();

        if (TemplateId == 0)
            return;

        Id = stream.ReadUInt64();
        Grade = stream.ReadByte();
        ItemFlags = (ItemFlag)stream.ReadByte();
        Count = stream.ReadInt32();
        var detailType = stream.ReadByte();
        ReadDetails(stream);
        CreateTime = stream.ReadDateTime();
        LifespanMins = stream.ReadInt32();
        MadeUnitId = stream.ReadUInt32();
        WorldId = stream.ReadByte();
        UnsecureTime = stream.ReadDateTime();
        UnpackTime = stream.ReadDateTime();
    }

    public override void ReadDetails(PacketStream stream)
    {
        if (stream.LeftBytes < DetailBytesLength)
            return;
        ImageItemTemplateId = stream.ReadUInt32();
        Durability = stream.ReadByte();
        stream.ReadInt16();
        RuneId = stream.ReadUInt32();

        ChargeStartTime = stream.ReadDateTime();
        DyeItemId = stream.ReadUInt32();

        for (var i = 0; i < GemIds.Length; i++)
            GemIds[i] = stream.ReadUInt32();

        TemperPhysical = stream.ReadUInt16();
        TemperMagical = stream.ReadUInt16();
    }

    public override void WriteDetails(PacketStream stream)
    {
        stream.Write(ImageItemTemplateId);
        stream.Write(Durability);
        stream.Write((short)0);
        stream.Write(RuneId);

        stream.Write(Template.BindType == ItemBindType.BindOnUnpack ? UnpackTime : ChargeStartTime);
        stream.Write(DyeItemId);

        foreach (var gemId in GemIds)
            stream.Write(gemId);

        stream.Write(TemperPhysical);
        stream.Write(TemperMagical);
    }
}
