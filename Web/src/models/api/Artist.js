import Model from './Model';
import ArtistMember from './ArtistMember';

export default class Artist extends Model {
    resource() {
        return 'artists';
      }

      members() {
        return this.hasMany(ArtistMember);
      }

      relations() {
        return {
          'members': ArtistMember
        }
      }
}