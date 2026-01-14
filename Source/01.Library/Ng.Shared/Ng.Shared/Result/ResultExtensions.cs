namespace Ng.Shared.Result
{
    public static class ResultExtensions
    {
        /// <summary>
        /// Result를 HTTP 응답으로 변환할 때 필요한 정보 추출
        /// </summary>
        public static HttpResponseInfo ToHttpResponse<T>(this Result<T> result)
        {
            return new HttpResponseInfo
            {
                StatusCode = result.ResultData.DetailCode.ToHttpStatusCode(),
                IsSuccess = result.IsSuccess,
                Message = result.ResultData.DetailCode.GetUserFriendlyMessage(result.Message),
                Code = result.ResultData.DetailCode.Name,
                Value = result.Value,
                Details = result.ResultData.Details,
                Timestamp = DateTime.Now
            };
        }

        /// <summary>
        /// Result를 HTTP 응답으로 변환할 때 필요한 정보 추출 (비제네릭)
        /// </summary>
        public static HttpResponseInfo ToHttpResponse(this Result result)
        {
            return new HttpResponseInfo
            {
                StatusCode = result.ResultData.DetailCode.ToHttpStatusCode(),
                IsSuccess = result.IsSuccess,
                Message = result.ResultData.DetailCode.GetUserFriendlyMessage(result.Message),
                Code = result.ResultData.DetailCode.Name,
                Value = null,
                Details = result.ResultData.Details,
                Timestamp = DateTime.Now
            };
        }

        /// <summary>
        /// 로깅을 위한 정보 추출
        /// </summary>
        public static LogInfo ToLogInfo<T>(this Result<T> result, string? operationName = null)
        {
            return new LogInfo
            {
                Level = result.ResultData.DetailCode.GetLogLevel(),
                Category = result.ResultData.DetailCode.GetMetricCategory(),
                Code = result.ResultData.DetailCode.Name,
                Message = result.Message,
                Details = result.ResultData.Details,
                OperationName = operationName,
                IsSuccess = result.IsSuccess,
                Priority = result.ResultData.DetailCode.GetPriority(),
                RequiresNotification = result.ResultData.DetailCode.RequiresNotification(),
                Timestamp = DateTime.Now
            };
        }

        /// <summary>
        /// 로깅을 위한 정보 추출 (비제네릭)
        /// </summary>
        public static LogInfo ToLogInfo(this Result result, string? operationName = null)
        {
            return new LogInfo
            {
                Level = result.ResultData.DetailCode.GetLogLevel(),
                Category = result.ResultData.DetailCode.GetMetricCategory(),
                Code = result.ResultData.DetailCode.Name,
                Message = result.Message,
                Details = result.ResultData.Details,
                OperationName = operationName,
                IsSuccess = result.IsSuccess,
                Priority = result.ResultData.DetailCode.GetPriority(),
                RequiresNotification = result.ResultData.DetailCode.RequiresNotification(),
                Timestamp = DateTime.Now
            };
        }

        /// <summary>
        /// 메트릭 수집을 위한 정보 추출
        /// </summary>
        public static MetricInfo ToMetricInfo<T>(this Result<T> result, string operationName, TimeSpan? duration = null)
        {
            return new MetricInfo
            {
                OperationName = operationName,
                Category = result.ResultData.DetailCode.GetMetricCategory(),
                Code = result.ResultData.DetailCode.Name,
                IsSuccess = result.IsSuccess,
                IsRetryable = result.ResultData.DetailCode.IsRetryable(),
                Priority = result.ResultData.DetailCode.GetPriority(),
                Duration = duration ?? TimeSpan.Zero,
                Timestamp = DateTime.Now
            };
        }

        /// <summary>
        /// 조건부 실행 - 성공 시에만 실행
        /// </summary>
        public static async Task<Result<T>> OnSuccessAsync<T>(this Result<T> result, Func<T, Task> action)
        {
            if (result.IsSuccess)
            {
                await action(result.Value!);
            }
            return result;
        }

        /// <summary>
        /// 조건부 실행 - 실패 시에만 실행
        /// </summary>
        public static async Task<Result<T>> OnErrorAsync<T>(this Result<T> result, Func<ResultInfo, Task> action)
        {
            if (result.IsError)
            {
                await action(result.ResultData);
            }
            return result;
        }

        /// <summary>
        /// 조건부 실행 - 특정 에러 코드일 때만 실행
        /// </summary>
        public static async Task<Result<T>> OnErrorCodeAsync<T>(this Result<T> result, ErrorCode errorCode, Func<ResultInfo, Task> action)
        {
            if (result.IsError && result.ResultData.DetailCode == errorCode)
            {
                await action(result.ResultData);
            }
            return result;
        }

        /// <summary>
        /// 재시도 가능한 에러인지 확인
        /// </summary>
        public static bool IsRetryable<T>(this Result<T> result)
        {
            return result.IsError && result.ResultData.DetailCode.IsRetryable();
        }

        /// <summary>
        /// 사용자에게 표시 가능한 결과인지 확인
        /// </summary>
        public static bool IsUserFriendly<T>(this Result<T> result)
        {
            return result.ResultData.DetailCode.IsUserFriendly();
        }

        /// <summary>
        /// 알림이 필요한 결과인지 확인
        /// </summary>
        public static bool RequiresNotification<T>(this Result<T> result)
        {
            return result.ResultData.DetailCode.RequiresNotification();
        }

        /// <summary>
        /// Result를 다른 타입으로 변환 (값이 있을 때만)
        /// </summary>
        public static Result<TNew> MapWhenValue<T, TNew>(this Result<T> result, Func<T, TNew> mapper, TNew defaultValue = default!)
        {
            if (result.IsSuccess && result.Value != null)
            {
                try
                {
                    var newValue = mapper(result.Value);
                    return Result<TNew>.Success(newValue);
                }
                catch (Exception ex)
                {
                    return Result<TNew>.FromException(ex);
                }
            }

            if (result.IsWarning && result.Value != null)
            {
                try
                {
                    var newValue = mapper(result.Value);
                    return Result<TNew>.Warning(newValue, (WarningCode)result.ResultData.DetailCode, result.Message, result.ResultData.Details);
                }
                catch (Exception ex)
                {
                    return Result<TNew>.FromException(ex);
                }
            }

            if (result.IsInformation && result.Value != null)
            {
                try
                {
                    var newValue = mapper(result.Value);
                    return Result<TNew>.Information(newValue, (InformationCode)result.ResultData.DetailCode, result.Message, result.ResultData.Details);
                }
                catch (Exception ex)
                {
                    return Result<TNew>.FromException(ex);
                }
            }

            // 값이 없거나 에러인 경우
            return Result<TNew>.Failure(result.ResultData);
        }

        /// <summary>
        /// 여러 Result 결합 - 모두 성공해야 성공
        /// </summary>
        public static Result<T[]> CombineAll<T>(params Result<T>[] results)
        {
            var errors = results.Where(r => r.IsError).ToList();
            if (errors.Any())
            {
                var firstError = errors.First();
                return Result<T[]>.Failure(firstError.ResultData);
            }

            var warnings = results.Where(r => r.IsWarning).ToList();
            if (warnings.Any())
            {
                var values = results.Select(r => r.Value!).ToArray();
                var firstWarning = warnings.First();
                return Result<T[]>.Warning(values, (WarningCode)firstWarning.ResultData.DetailCode,
                    $"Combined with {warnings.Count} warnings", firstWarning.ResultData.Details);
            }

            var successValues = results.Select(r => r.Value!).ToArray();
            return Result<T[]>.Success(successValues);
        }

        /// <summary>
        /// 여러 Result 결합 - 적어도 하나가 성공하면 성공
        /// </summary>
        public static Result<T> CombineAny<T>(params Result<T>[] results)
        {
            var successes = results.Where(r => r.IsSuccess).ToList();
            if (successes.Any())
            {
                return successes.First();
            }

            var warnings = results.Where(r => r.IsWarning).ToList();
            if (warnings.Any())
            {
                return warnings.First();
            }

            // 모두 실패한 경우 첫 번째 에러 반환
            return results.First();
        }
    }

    /// <summary>
    /// HTTP 응답 정보
    /// </summary>
    public class HttpResponseInfo
    {
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public object? Value { get; set; }
        public string? Details { get; set; }
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// 로그 정보
    /// </summary>
    public class LogInfo
    {
        public string Level { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
        public string? OperationName { get; set; }
        public bool IsSuccess { get; set; }
        public int Priority { get; set; }
        public bool RequiresNotification { get; set; }
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// 메트릭 정보
    /// </summary>
    public class MetricInfo
    {
        public string OperationName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public bool IsRetryable { get; set; }
        public int Priority { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
