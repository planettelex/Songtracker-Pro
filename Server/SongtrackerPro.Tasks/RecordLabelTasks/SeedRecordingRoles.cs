using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface ISeedRecordingRolesTask : ITask<Nothing, bool> { }

    public class SeedRecordingRoles : TaskBase, ISeedRecordingRolesTask
    {
        public SeedRecordingRoles(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(Nothing nothing)
        {
            try
            {
                var recordingRoles = _dbContext.RecordingRoles.ToList();
                if (recordingRoles.Any())
                    return new TaskResult<bool>(false);

                _dbContext.RecordingRoles.Add(new RecordingRole { Name = SeedData("RECORDING_ROLE_PRODUCER")} );
                _dbContext.RecordingRoles.Add(new RecordingRole { Name = SeedData("RECORDING_ROLE_RECORDING_ENGINEER")} );
                _dbContext.RecordingRoles.Add(new RecordingRole { Name = SeedData("RECORDING_ROLE_MIXING_ENGINEER")} );
                _dbContext.RecordingRoles.Add(new RecordingRole { Name = SeedData("RECORDING_ROLE_MASTERING_ENGINEER")} );
                _dbContext.RecordingRoles.Add(new RecordingRole { Name = SeedData("RECORDING_ROLE_PROGRAMMER")} );
                _dbContext.RecordingRoles.Add(new RecordingRole { Name = SeedData("RECORDING_ROLE_TURNTABLIST")} );
                _dbContext.RecordingRoles.Add(new RecordingRole { Name = SeedData("RECORDING_ROLE_DRUMMER")} );
                _dbContext.RecordingRoles.Add(new RecordingRole { Name = SeedData("RECORDING_ROLE_PERCUSSIONIST")} );
                _dbContext.RecordingRoles.Add(new RecordingRole { Name = SeedData("RECORDING_ROLE_BASSIST")} );
                _dbContext.RecordingRoles.Add(new RecordingRole { Name = SeedData("RECORDING_ROLE_RHYTHM_GUITARIST")} );
                _dbContext.RecordingRoles.Add(new RecordingRole { Name = SeedData("RECORDING_ROLE_LEAD_GUITARIST")} );
                _dbContext.RecordingRoles.Add(new RecordingRole { Name = SeedData("RECORDING_ROLE_VIOLINIST")} );
                _dbContext.RecordingRoles.Add(new RecordingRole { Name = SeedData("RECORDING_ROLE_CELLIST")} );
                _dbContext.RecordingRoles.Add(new RecordingRole { Name = SeedData("RECORDING_ROLE_OTHER_STRINGS")} );
                _dbContext.RecordingRoles.Add(new RecordingRole { Name = SeedData("RECORDING_ROLE_PIANIST")} );
                _dbContext.RecordingRoles.Add(new RecordingRole { Name = SeedData("RECORDING_ROLE_KEYBOARDIST")} );
                _dbContext.RecordingRoles.Add(new RecordingRole { Name = SeedData("RECORDING_ROLE_LEAD_VOCALIST")} );
                _dbContext.RecordingRoles.Add(new RecordingRole { Name = SeedData("RECORDING_ROLE_BACKING_VOCALIST")} );
                _dbContext.RecordingRoles.Add(new RecordingRole { Name = SeedData("RECORDING_ROLE_TRUMPETER")} );
                _dbContext.RecordingRoles.Add(new RecordingRole { Name = SeedData("RECORDING_ROLE_SAXOPHONIST")} );
                _dbContext.RecordingRoles.Add(new RecordingRole { Name = SeedData("RECORDING_ROLE_CLARINETIST")} );
                _dbContext.RecordingRoles.Add(new RecordingRole { Name = SeedData("RECORDING_ROLE_TROMBONIST")} );
                _dbContext.RecordingRoles.Add(new RecordingRole { Name = SeedData("RECORDING_ROLE_HARMONICA")} );
                _dbContext.RecordingRoles.Add(new RecordingRole { Name = SeedData("RECORDING_ROLE_OTHER_WIND")} );
                _dbContext.RecordingRoles.Add(new RecordingRole { Name = SeedData("RECORDING_ROLE_OTHER_BRASS")} );
                _dbContext.RecordingRoles.Add(new RecordingRole { Name = SeedData("RECORDING_ROLE_OTHER_INSTRUMENT")} );

                return new TaskResult<bool>(true);
            }
            catch (Exception e)
            {
                return new TaskResult<bool>(new TaskException(e));
            }
        }
    }
}
