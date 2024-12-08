using System.ComponentModel;

namespace Common.Enums.TeacherEnum;

public enum TeacherFieldEnum
{
    [Description("مهندسی کامپیوتر")] ComputerEng = 1,
    [Description("مهندسی برق")] ElectricalEng = 2,
    [Description("مهندسی شیمی")] ChemistryEng = 3,
    [Description("مهندسی عمران")] CivilEng = 4,
    [Description("مهندسی مکانیک")] MechanicalEng = 5,
}