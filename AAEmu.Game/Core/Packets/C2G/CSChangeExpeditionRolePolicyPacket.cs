using AAEmu.Commons.Network;
using AAEmu.Game.Core.Network.Game;
using AAEmu.Game.Models.Game.Expeditions;

namespace AAEmu.Game.Core.Packets.C2G
{
    public class CSChangeExpeditionRolePolicyPacket : GamePacket
    {
        public CSChangeExpeditionRolePolicyPacket() : base(0x006, 1)
        {
        }

        public override void Read(PacketStream stream)
        {
            var rolePolicy = new ExpeditionRolePolicy();
            rolePolicy.Read(stream);

            _log.Debug("ChangeExpeditionRolePolicy, Id: {0}, Role: {1}", rolePolicy.Id, rolePolicy.Role);
        }
    }
}
