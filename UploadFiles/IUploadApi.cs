using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UploadFiles
{
	public interface IUploadApi
	{
		public Task UploadAsync(string localFilePath);
	}
}
