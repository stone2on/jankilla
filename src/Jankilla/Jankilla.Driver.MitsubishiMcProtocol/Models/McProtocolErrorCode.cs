using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jankilla.Driver.MitsubishiMcProtocol.Models
{
    public class McProtocolErrorCode
    {
        private static McProtocolErrorCode _instance;

        public static McProtocolErrorCode Instance
        {
            get
            {
                if (null == _instance)
                    _instance = new McProtocolErrorCode();

                return _instance;
            }
        }

        //------------------------------
        private Dictionary<ushort, Error> _errorCodeMap = new Dictionary<ushort, Error>();

        protected McProtocolErrorCode()
        {
            Initialize();
        }

        public void Initialize()
        {
            //------------------------------
            for (byte i = 0x40; i <= 0x4F; ++i)
            {
                for (byte j = 0x00; j <= 0xFF; ++j)
                {
                    var newElement = new Error();
                    newElement.ErrorCode[0] = i;
                    newElement.ErrorCode[1] = j;
                    newElement.Description = "CPU 모듈이 검출한 에러(MC 프로토콜에 의한 통신 기능 이외에서 발생한 에러)";
                    newElement.Solution = "QCPU 사용자 매뉴얼(하드웨어 설계/보수 점검편)을 참조하여 처리한다.";

                    ushort v1 = (ushort)((j << 8) | i);
                    newElement.Value = v1;

                    _errorCodeMap.Add(v1, newElement);
                }
            }
            //------------------------------
            {
                var newElement = new Error();
                newElement.ErrorCode[0] = 0x00;
                newElement.ErrorCode[1] = 0x55;
                newElement.Description = "RUN 중 쓰기를 허가로 하지 않은 경우에 상대 기기에 의해 CPU 모듈이 RUN 중에 데이터의 쓰기를 요구하였다";
                newElement.Solution = "RUN 중 쓰기를 허가로 설정 후 데이터를 쓴다 / CPU 모듈을 STOP으로 하고 데이터를 쓴다";

                ushort v1 = (ushort)(newElement.ErrorCode[1] << 8 | newElement.ErrorCode[0]);
                newElement.Value = v1;

                _errorCodeMap.Add(v1, newElement);
            }
            //------------------------------
            {
                var newElement = new Error();
                newElement.ErrorCode[0] = 0xC0;
                newElement.ErrorCode[1] = 0x50;
                newElement.Description = "Ethernet 포트 내장 QCPU의 교신 데이터 코드 설정에서 ASCII 코드 교신 설정 시 바이너리 코드로 변환할 수 없는 ASCII 코드의 데이터를 수신하였다.";
                newElement.Solution = "교신 데이터 코드 설정에서 바이너리 코드 교신을 설정 후 다시 Ethernet 포트 내장 QCPU를 기동하여 교신한다 / 상대 기기에서의 송신 데이터를 수정하여 송신한다";

                ushort v1 = (ushort)((newElement.ErrorCode[1] << 8) | newElement.ErrorCode[0]);
                newElement.Value = v1;

                _errorCodeMap.Add(v1, newElement);
            }
            //------------------------------
            for (byte i = 0x51; i <= 0x54; ++i)
            {
                var newElement = new Error();
                newElement.ErrorCode[0] = 0xC0;
                newElement.ErrorCode[1] = i;
                newElement.Description = "읽기/쓰기 점수가 허용 범위를 벗어난다";
                newElement.Solution = "읽기/쓰기 점수를 수정하여 다시 Ethernet 포트 내장 QCPU에 송신한다";

                ushort v1 = (ushort)((newElement.ErrorCode[1] << 8) | newElement.ErrorCode[0]);
                newElement.Value = v1;

                _errorCodeMap.Add(v1, newElement);
            }
            //------------------------------
            {
                var newElement = new Error();
                newElement.ErrorCode[0] = 0xC0;
                newElement.ErrorCode[1] = 0x56;
                newElement.Description = "최대 어드레스를 초과하는 읽기/쓰기 요구를 하였다";
                newElement.Solution = "선두 어드레스 또는 읽기/쓰기 점수를 수정하여 다시 Ethernet 포트 내장 QCPU에 송신한다. (최대 어드레스를 초과하지 않게 한다.)";

                ushort v1 = (ushort)((newElement.ErrorCode[1] << 8) | newElement.ErrorCode[0]);
                newElement.Value = v1;

                _errorCodeMap.Add(v1, newElement);
            }
            //------------------------------
            {
                var newElement = new Error();
                newElement.ErrorCode[0] = 0xC0;
                newElement.ErrorCode[1] = 0x58;
                newElement.Description = "ASCII － 바이너리 변환 후의 요구 데이터 길이가 캐릭터부(텍스트의 일부)의 데이터수와 맞지 않는다";
                newElement.Solution = "텍스트부의 내용 또는 헤더부의 요구 데이터 길이를 검토 및 수정 후 다시 Ethernet 포트 내장 QCPU에 송신한다.";

                ushort v1 = (ushort)((newElement.ErrorCode[1] << 8) | newElement.ErrorCode[0]);
                newElement.Value = v1;

                _errorCodeMap.Add(v1, newElement);
            }
            //------------------------------
            {
                var newElement = new Error();
                newElement.ErrorCode[0] = 0xC0;
                newElement.ErrorCode[1] = 0x59;
                newElement.Description = "커맨드, 서브 커맨드가 잘못 지정되어 있다 / Ethernet 포트 내장 QCPU에서는 사용할 수 없는 커맨드, 서브 커맨드다";
                newElement.Solution = "요구 내용을 검토한다 / Ethernet 포트 내장 QCPU에서 사용할 수 있는 커맨드, 서브 커맨드를 송신한다";

                ushort v1 = (ushort)((newElement.ErrorCode[1] << 8) | newElement.ErrorCode[0]);
                newElement.Value = v1;

                _errorCodeMap.Add(v1, newElement);
            }
            //------------------------------
            {
                var newElement = new Error();
                newElement.ErrorCode[0] = 0xC0;
                newElement.ErrorCode[1] = 0x5B;
                newElement.Description = "지정 디바이스에 대해서 Ethernet 포트 내장 QCPU를읽거나 쓸 수 없다";
                newElement.Solution = "읽기/쓰기 하는 디바이스를 검토한다";

                ushort v1 = (ushort)((newElement.ErrorCode[1] << 8) | newElement.ErrorCode[0]);
                newElement.Value = v1;

                _errorCodeMap.Add(v1, newElement);
            }
            //------------------------------
            {
                var newElement = new Error();
                newElement.ErrorCode[0] = 0xC0;
                newElement.ErrorCode[1] = 0x5C;
                newElement.Description = "요구 내용에 잘못이 있다. (워드 디바이스에 대한 비트단위의 읽기/ 쓰기 시)";
                newElement.Solution = "요구 내용을 수정하여 다시 Ethernet 포트 내장 QCPU 에 송신한다. (서브 커맨드의 수정 등)";

                ushort v1 = (ushort)((newElement.ErrorCode[1] << 8) | newElement.ErrorCode[0]);
                newElement.Value = v1;

                _errorCodeMap.Add(v1, newElement);
            }
            //------------------------------
            {
                var newElement = new Error();
                newElement.ErrorCode[0] = 0xC0;
                newElement.ErrorCode[1] = 0x5D;
                newElement.Description = "모니터 등록이 되어 있지 않다";
                newElement.Solution = "모니터 등록을 하고 나서 모니터를 실행한다";

                ushort v1 = (ushort)((newElement.ErrorCode[1] << 8) | newElement.ErrorCode[0]);
                newElement.Value = v1;

                _errorCodeMap.Add(v1, newElement);
            }
            //------------------------------
            {
                var newElement = new Error();
                newElement.ErrorCode[0] = 0xC0;
                newElement.ErrorCode[1] = 0x5F;
                newElement.Description = "대상 CPU 모듈에 대해서 실행할 수 없는 요구다";
                newElement.Solution = "네트워크 번호, PLC 번호, 요구 상대 모듈 I/O 번호, 요구 상대 모듈 국번호를 수정한다 / 읽기/쓰기 요구의 내용을 수정한다";

                ushort v1 = (ushort)((newElement.ErrorCode[1] << 8) | newElement.ErrorCode[0]);
                newElement.Value = v1;

                _errorCodeMap.Add(v1, newElement);
            }
            //------------------------------
            {
                var newElement = new Error();
                newElement.ErrorCode[0] = 0xC0;
                newElement.ErrorCode[1] = 0x60;
                newElement.Description = "요구 내용에 잘못이 있다. (비트 디바이스에 대한 데이터의 지정에 잘못이 있는 등)";
                newElement.Solution = "요구 내용을 수정하여 다시 Ethernet 포트 내장 QCPU 에 송신한다. (데이터의 수정 등)";

                ushort v1 = (ushort)((newElement.ErrorCode[1] << 8) | newElement.ErrorCode[0]);
                newElement.Value = v1;

                _errorCodeMap.Add(v1, newElement);
            }
            //------------------------------
            {
                var newElement = new Error();
                newElement.ErrorCode[0] = 0xC0;
                newElement.ErrorCode[1] = 0x61;
                newElement.Description = "요구 데이터 길이가 캐릭터부(텍스트의 일부)의 데이터수와 맞지 않는다";
                newElement.Solution = "텍스트부의 내용 또는 헤더부의 요구 데이터 길이를 검토 및 수정 후 다시 Ethernet 포트 내장 QCPU에 송신한다";

                ushort v1 = (ushort)((newElement.ErrorCode[1] << 8) | newElement.ErrorCode[0]);
                newElement.Value = v1;

                _errorCodeMap.Add(v1, newElement);
            }
            //------------------------------
            {
                var newElement = new Error();
                newElement.ErrorCode[0] = 0xC0;
                newElement.ErrorCode[1] = 0x6F;
                newElement.Description = "교신 데이터 코드 설정이 바이너리일 때 ASCII의 요구스테이트먼트를 수신하였거나, 교신 데이터 코드 설정이 ASCII일 때 바이너리의 요구 스테이트먼트를 수신하였다.(이 에러 코드는 에러 이력만 등록되고 이상 응답은 반환되지 않는다.)";
                newElement.Solution = "교신 데이터 코드 설정에 있는 요구 스테이트먼트를 송신한다 / 요구 스테이트먼트에 있는 교신 데이터 코드 설정으로 변경한다";

                ushort v1 = (ushort)((newElement.ErrorCode[1] << 8) | newElement.ErrorCode[0]);
                newElement.Value = v1;

                _errorCodeMap.Add(v1, newElement);
            }
            //------------------------------
            {
                var newElement = new Error();
                newElement.ErrorCode[0] = 0xC0;
                newElement.ErrorCode[1] = 0x70;
                newElement.Description = "대상국에 대해서는 디바이스 메모리의 확장 지정은 할수 없다";
                newElement.Solution = "확장 지정을 하지 않고 읽기/쓰기 한다";

                ushort v1 = (ushort)((newElement.ErrorCode[1] << 8) | newElement.ErrorCode[0]);
                newElement.Value = v1;

                _errorCodeMap.Add(v1, newElement);
            }
            //------------------------------
            {
                var newElement = new Error();
                newElement.ErrorCode[0] = 0xC0;
                newElement.ErrorCode[1] = 0xB5;
                newElement.Description = "CPU 모듈에서 취급할 수 없는 데이터가 지정되었다";
                newElement.Solution = "요구 내용을 검토한다 / 현재의 요구를 중지한다";

                ushort v1 = (ushort)((newElement.ErrorCode[1] << 8) | newElement.ErrorCode[0]);
                newElement.Value = v1;

                _errorCodeMap.Add(v1, newElement);
            }
            //------------------------------
            {
                var newElement = new Error();
                newElement.ErrorCode[0] = 0xC2;
                newElement.ErrorCode[1] = 0x00;
                newElement.Description = "리모트 패스워드에 잘못이 있다";
                newElement.Solution = "리모트 패스워드를 검토하여 리모트 패스워드의 해제처리 / 잠금 처리를 재실행한다";

                ushort v1 = (ushort)((newElement.ErrorCode[1] << 8) | newElement.ErrorCode[0]);
                newElement.Value = v1;

                _errorCodeMap.Add(v1, newElement);
            }
            //------------------------------
            {
                var newElement = new Error();
                newElement.ErrorCode[0] = 0xC2;
                newElement.ErrorCode[1] = 0x01;
                newElement.Description = "교신에 사용한 포트가 리모트 패스워드로 잠금 상태이거나 교신 데이터 코드 설정이 ASCII 코드일 때 리모트 패스워드에 의해 잠금 상태이므로 서브 커맨드 이후를 바이너리 코드로 변환할 수 없다";
                newElement.Solution = "리모트 패스워드의 해제 처리를 한 후에 교신한다";

                ushort v1 = (ushort)((newElement.ErrorCode[1] << 8) | newElement.ErrorCode[0]);
                newElement.Value = v1;

                _errorCodeMap.Add(v1, newElement);
            }
            //------------------------------
            {
                var newElement = new Error();
                newElement.ErrorCode[0] = 0xC2;
                newElement.ErrorCode[1] = 0x04;
                newElement.Description = "리모트 패스워드의 해제 처리를 요구한 기기와 다르다";
                newElement.Solution = "리모트 패스워드의 해제 처리를 요구한 상대 기기에서 리모트 패스워드의 잠금 처리를 요구한다";

                ushort v1 = (ushort)((newElement.ErrorCode[1] << 8) | newElement.ErrorCode[0]);
                newElement.Value = v1;

                _errorCodeMap.Add(v1, newElement);
            }
        }

        public Error GetErrorOrNull(int key)
        {
            _errorCodeMap.TryGetValue((ushort)key, out Error val);

            return val;
        }
    }
    public class Error
    {
        // MC Protocol 에서 EX) 51 C0 받았으면 C0 51 로 넣을 것
        public byte[] ErrorCode = new byte[2];

        //------------------------------
        // 에러 내용
        public string Description = string.Empty;

        //------------------------------
        // 처리 방법
        public string Solution = string.Empty;

        public ushort Value;
    }
}
