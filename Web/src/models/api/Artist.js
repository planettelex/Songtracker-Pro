import Model from './Model';
import ArtistMember from './ArtistMember';
import ArtistManager from './ArtistManager';
import ArtistAccount from './ArtistAccount';

export default class Artist extends Model {
    resource() {
        return 'artists';
      }

      members() {
        return this.hasMany(ArtistMember);
      }

      managers() {
        return this.hasMany(ArtistManager);
      }

      accounts() {
        return this.hasMany(ArtistAccount);
      }
}