ALTER TABLE public."Accounts" ALTER COLUMN "Ac_Money" TYPE numeric;
ALTER TABLE public."MoneyAccount" ALTER COLUMN "Ma_Money" TYPE numeric;
ALTER TABLE public."MoneyAccount" ALTER COLUMN "Ma_Total" TYPE numeric;
ALTER TABLE public."ProfitSharing" ALTER COLUMN "Ps_MoneyValue" TYPE numeric;